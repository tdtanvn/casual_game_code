using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClanRightPanelUI : MonoBehaviour
{
    private Clan.State state => OnlineManager.Instance.playerDB.Clan.currentState;

    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI memberCount;
    [SerializeField] private Button joinBtn;
    [SerializeField] private Button createBtn;
    [SerializeField] private Button leaveBtn;
    private Clan currentClan = null;
    private ClanPopup popup = null;
    public void SetupUI(Clan clan, ClanPopup popup = null)
    {
        if (currentClan == clan) return;
        currentClan = clan;
        this.popup = popup;

        joinBtn.gameObject.SetActive(state == Clan.State.Out);
        createBtn.gameObject.SetActive(state == Clan.State.Out);
        leaveBtn.gameObject.SetActive(state == Clan.State.In);

        nameTxt.text= clan.name;
        description.text= clan.description;
        memberCount.text = clan.members.Count + "/" + clan.maxMember;

        joinBtn.onClick.RemoveAllListeners();
        createBtn.onClick.RemoveAllListeners();
        leaveBtn.onClick.RemoveAllListeners();
        joinBtn.onClick.AddListener(OnJoinBtnClick);
        createBtn.onClick.AddListener(OnCreateBtnClick);
        leaveBtn.onClick.AddListener(OnLeaveBtnClick);
    }

    private void OnJoinBtnClick()
    {
        if (state == Clan.State.Out)
        {
            var data = new CommonPopup.PopupData(title: "CLAN", description: $"Join clan {currentClan.name}", null, "OK", () => OnlineManager.Instance.playerDB.Clan.JoinClan(currentClan.id), "Cancel");
            GameManager.Instance.commonPopup.PushPopup(data);
        }
    }

    private void OnCreateBtnClick()
    {
        popup.PushPopup(ClanPopup.Type.Create);
        // if (state == Clan.State.Out)
        //     OnlineManager.Instance.playerDB.Clan.CreateClan(currentClan.id);
    }

    private void OnLeaveBtnClick()
    {
        if (state == Clan.State.In)
        {
            var data = new CommonPopup.PopupData(title: "CLAN", description: $"Leave clan {currentClan.name}", null, "OK", () => OnlineManager.Instance.playerDB.Clan.LeaveClan(currentClan.id), "Cancel");
            GameManager.Instance.commonPopup.PushPopup(data);
        }
    }
}
