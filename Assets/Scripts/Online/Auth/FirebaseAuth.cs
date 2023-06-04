using System;
using UnityEngine;

public static class FirebaseAuth
{
#if UNITY_WEBGL && !UNITY_EDITOR
    private static string requestUri => GoogleAuth.RedirectUri;
#else
    private static readonly string requestUri = $"http://localhost";
#endif
    private const string ApiKey = "AIzaSyAPfv9L33dElRJg4Urv7_DxJNDV_LIVIpU";

    public static void SingInWithToken(string token, string providerId, Action<FirebaseUser> callback)
    {
        var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithIdp?key={ApiKey}";
        var payLoad = $"{{\"postBody\":\"id_token={token}&providerId={providerId}\",\"requestUri\":\"{requestUri}\",\"returnIdpCredential\":true,\"returnSecureToken\":true}}";

        OnlineManager.Instance.PostRequest(url, payLoad, json =>
        {
            try
            {
                FirebaseUser user = FirebaseUser.FromJson(json);
                callback(user);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        });
    }
}

[Serializable]
public class FirebaseUser
{
    public string federatedId;
    public string providerId;
    public string localId;
    public bool emailVerified;
    public string email;
    public string oauthIdToken;
    public string oauthAccessToken;
    public string oauthTokenSecret;
    public string rawUserInfo;
    public string firstName;
    public string lastName;
    public string fullName;
    public string displayName;
    public string photoUrl;
    public string idToken;
    public string refreshToken;
    public string expiresIn;
    public bool needConfirmation;

    public static FirebaseUser FromJson(string json)
    {
        return JsonUtility.FromJson<FirebaseUser>(json);
    }
}
