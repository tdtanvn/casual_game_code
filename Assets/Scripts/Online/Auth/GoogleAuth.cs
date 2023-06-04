using System;
using UnityEngine;

public static class GoogleAuth
{
    private const string ClientId = "268833587453-im3c7tg3tlovnjnmbc95asp0ho51c5bk.apps.googleusercontent.com";
    private const string ClientSecret = "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";

#if UNITY_WEBGL && !UNITY_EDITOR
    public static string RedirectUri
    {
        get
        {
            var url = Application.absoluteURL; 
            
            int codeIndex = url.IndexOf("?code=");
            if (codeIndex >= 0)
            {
                url = url.Substring(0, codeIndex);
            }
            url = url.TrimEnd('/');

            return url;
        }
    }
#else
    private const int Port = 1234;
    private static readonly string RedirectUri = $"http://localhost:{Port}";

    private static readonly HttpCodeListener codeListener = new HttpCodeListener(Port);
#endif
    private static string loginUrl = $"https://accounts.google.com/o/oauth2/v2/auth?client_id={ClientId}&redirect_uri={RedirectUri}&response_type=code&scope=email";

    public static void Authenticate(Action<FirebaseUser> cb)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        PlayerPrefs.SetInt("isWaitingForLogin", 1);
        Application.ExternalEval($"window.location.href = '{loginUrl}';");
#else
        Application.OpenURL(loginUrl);

        codeListener.StartListening(code =>
        {
            AuthenticateWithCode(code, cb);

            codeListener.StopListening();
        });
#endif
    }

    public static void AuthenticateWithCode(string code, Action<FirebaseUser> cb)
    {
        ExchangeAuthCodeWithIdToken(code, idToken =>
        {
            FirebaseAuth.SingInWithToken(idToken, "google.com", user => cb(user));
        });
    }

    private static void ExchangeAuthCodeWithIdToken(string code, Action<string> callback)
    {
        try
        {
            var url = "https://oauth2.googleapis.com/token";
            var payload = $"{{\"code\":\"{code}\",\"client_id\":\"{ClientId}\",\"client_secret\":\"{ClientSecret}\",\"redirect_uri\":\"{RedirectUri}\",\"grant_type\":\"authorization_code\"}}";

            OnlineManager.Instance.PostRequest(url, payload, json =>
            {
                try
                {
                    var data = JsonUtility.FromJson<GoogleIdTokenResponse>(json);
                    callback(data.id_token);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            });
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}

[Serializable]
public class GoogleIdTokenResponse
{
    public string id_token;
}
