using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerProfilePopup : MonoBehaviour
{
    PlayerProfile profile => OnlineManager.Instance.playerDB.PlayerProfile;

    [SerializeField] TextMeshProUGUI mainMenuPlayerNameText;
    [SerializeField] TextMeshProUGUI popupPlayerNameText;
    [SerializeField] GameObject changeNamePopup;
    [SerializeField] TMP_InputField changeNameText;

    private void Start()
    {
        SetupUI();
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void SetupUI()
    {
        mainMenuPlayerNameText.text = popupPlayerNameText.text = string.IsNullOrEmpty(profile.PlayerName) ? profile.PlayerId : profile.PlayerName;
    }

    public void SetPlayerInfo(string name)
    {
        OnlineManager.Instance.playerDB.SetPlayerInfo(name, null, SetupUI);
    }

    public void OnEditNameClick()
    {
        changeNameText.text = profile.PlayerName;
        MainMenuManager.Push(changeNamePopup);
    }

    public void OnConfirmNameClick()
    {
        if (string.IsNullOrWhiteSpace(changeNameText.text) || changeNameText.text == profile.PlayerName)
        {
            return;
        }

        SetPlayerInfo(changeNameText.text);
        MainMenuManager.Back();
    }
}
