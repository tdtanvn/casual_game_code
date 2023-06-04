using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClanSlotUIOutState : ClanSlotUI
{
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI memberCount;
    [SerializeField] private TextMeshProUGUI requiredLevelTxt;
    [SerializeField] private Button selectBtn;
    private Clan clan = null;
    private ClanRightPanelUI rightPanel = null;

    public void SetupUI(ClanSlotOutState slot, ClanRightPanelUI rightPanel)
    {
        if (slot == null || this.clan == slot.clan) return;

        this.clan = slot.clan;
        this.rightPanel = rightPanel;

        nameTxt.text = clan.name;
        memberCount.text = clan.members.Count + "/" + clan.maxMember;
        requiredLevelTxt.text = "Required level " + slot.clan.requiredLevel;
        selectBtn.onClick.RemoveAllListeners();
        selectBtn.onClick.AddListener(() => SetupRightPanel(this.clan));
    }

    private void SetupRightPanel(Clan clan)
    {
        rightPanel.SetupUI(clan);
    }
}
