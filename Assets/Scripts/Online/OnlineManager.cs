using UnityEngine;
using OneB;
using System.Collections;
using System;
using Google.Protobuf.Collections;
using UnityEngine.Events;
using Random = System.Random;
using System.Linq;
using UnityEngine.Networking;

public class OnlineManager : MonoBehaviour
{
    static OnlineManager s_Instance;
    public static OnlineManager Instance => s_Instance;
    public string GameId;
    public GameEnvironment Environment;
    public string GameVersion;

    [SerializeField] string m_SaveFilename = "save.dat";

    public bool m_Initialzed = false;

    struct LoginInfo
    {
        public string playerId;
        public string secretKey;
        public string sub;
    };
    LoginInfo loginInfo;
    bool isAuthCreated;
    public OneBServicesClient API
    {
        get;
        private set;
    }

    public GameDB GameDB { get; private set; }
    public PlayerDB playerDB { get; private set; }

    private void Awake()
    {
        SetupInstance();

#if UNITY_WEBGL && !UNITY_EDITOR
        if (PlayerPrefs.GetInt("isWaitingForLogin", 0) > 0)
        {
            PlayerPrefs.SetInt("isWaitingForLogin", 0);

            string url = Application.absoluteURL;
            if (url.Contains("?code="))
            {
                string code = Uri.UnescapeDataString(GetParameterValue(url, "code"));
                Debug.Log("Code Fetched: " + code);

                GoogleAuth.AuthenticateWithCode(code, user => OnlineManager.Instance.LinkWithFirebaseUser(user));
            }
        }
#endif

        Messenger.OnLoginDone += GetPlayerDB;

        StartSession();
    }

#if UNITY_WEBGL && !UNITY_EDITOR
    private string GetParameterValue(string url, string parameterName)
    {
        Uri uri = new Uri(url);
        string query = uri.Query;
        string[] parameters = query.TrimStart('?').Split('&');

        foreach (string parameter in parameters)
        {
            string[] parts = parameter.Split('=');
            if (parts.Length == 2 && parts[0] == parameterName)
            {
                return parts[1];
            }
        }

        return null;
    }
#endif

    void SetupInstance()
    {
        if (s_Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            s_Instance = this;
        }
        API = new OneBServicesClient { GameId = GameId, Environment = Environment, GameVersion = GameVersion, DebugLogEnabled = true };

    }

    public static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private async void StartSession()
    {
        (loginInfo, isAuthCreated) = GetOrCreateAuth();
        if (isAuthCreated)
        {
            await Instance.API.Login(playerId: loginInfo.playerId, secretKey: loginInfo.secretKey);
            await Instance.API.Send<Empty>(new CallGameScriptCommand("Gameplay", "InitPlayerData"));
        }
        else
        {
            await Instance.API.Login(playerId: loginInfo.playerId, secretKey: loginInfo.secretKey);
        }

        GameDB = new GameDB();
    }

    public void Login()
    {
        Messenger.Broadcast(Messenger.OnLoginDone);
        MenuManager.LoadScene(BuiltInScenes.MainMenu, showLoading: true, loadingText: "Logging in", true);
        m_Initialzed = true;
    }

    public async void LinkWithFirebaseUser(FirebaseUser user)
    {
        Debug.LogFormat("User signed in successfully with Firebase: {0} ({1}) ({2})", user.displayName, user.localId, user.email);
        try
        {
            var result = await Instance.API.Send<LoginOutput>(new CallGameScriptCommand<LoginInput>("SocialAccount", "Login",
                                                   new LoginInput { IdToken = user.idToken, Provider = "firebase" }));
            if (result != null)
            {
                Debug.Log(result.AccessToken);
                Instance.API.AccessToken = result.AccessToken;
                loginInfo.sub = user.localId;
                FileManager.WriteToFile(m_SaveFilename, JsonUtility.ToJson(loginInfo));
            }

#if UNITY_WEBGL && !UNITY_EDITOR
            var data = new CommonPopup.PopupData(title: "LOGIN", description: $"Logged in successful to {user.email}", null, "OK", Login);

            GameManager.Instance.commonPopup.PushPopup(data);
#else
            Login();
#endif
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            var data = new CommonPopup.PopupData(title: "LOGIN", description: e.Message, null, "OK");

            GameManager.Instance.commonPopup.PushPopup(data);
        }
    }

    private (LoginInfo, bool) GetOrCreateAuth()
    {
        if (!FileManager.LoadFromFile(m_SaveFilename, out var jsonString))
        {
            var loginInfo = new LoginInfo();
            loginInfo.playerId = GenerateRandomString(10);
            loginInfo.secretKey = GenerateRandomString(10);
            FileManager.WriteToFile(m_SaveFilename, JsonUtility.ToJson(loginInfo));
            return (loginInfo, true);
        }
        return (JsonUtility.FromJson<LoginInfo>(jsonString), false);
    }

    private void GetPlayerDB()
    {
        playerDB = new PlayerDB();
    }

    public bool IsLinkedWithFirebaseAccount()
    {
        return !string.IsNullOrEmpty(loginInfo.sub);
    }

    public void PostRequest(string url, string payload, Action<string> callback)
    {
        Debug.Log($"[HTTP][POST] {url} {payload}");
        StartCoroutine(PostRequestInternal(url, payload, callback));
    }

    private IEnumerator PostRequestInternal(string url, string payload, Action<string> callback)
    {
        var request = new UnityWebRequest(url, "POST");

        request.SetRequestHeader("Content-Type", "application/json");
        request.downloadHandler = new DownloadHandlerBuffer();
        byte[] body = System.Text.Encoding.UTF8.GetBytes(payload);
        request.uploadHandler = new UploadHandlerRaw(body);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            Debug.Log($"[HTTP][{request.responseCode}] " + json);
            callback(json);
        }
        else
        {
            Debug.LogError($"[HTTP][{request.responseCode}] " + request.error);
        }
    }
}
