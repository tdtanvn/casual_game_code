using OneB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineLogin : MonoBehaviour
{
    public void StartPressed()
    {
        OnlineManager.Instance.Login();
    }

    public void GoogleLoginPressed()
    {
        if (OnlineManager.Instance.IsLinkedWithFirebaseAccount())
        {
            var data = new CommonPopup.PopupData(title: "LOGIN", description: "Already linked with an account", null, "OK");

            GameManager.Instance.commonPopup.PushPopup(data);
        }
        else
        {
            GoogleAuth.Authenticate(OnFirebaseAuthenticated);
        }
    }

    public void GuestLoginPressed()
    {

    }

    private void OnFirebaseAuthenticated(FirebaseUser user)
    {
        OnlineManager.Instance.LinkWithFirebaseUser(user);
    }
}
