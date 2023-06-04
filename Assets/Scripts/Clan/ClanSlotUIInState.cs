using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClanSlotUIInState : ClanSlotUI
{
    [SerializeField] private TextMeshProUGUI id;
    [SerializeField] private TextMeshProUGUI role;
    private Clan.Member member = null;

    public void SetupUI(ClanSlotInState slot)
    {
        if (slot == null || member == slot.member) return;

        member = slot.member;

        id.text = member.id; 
        role.text = member.role;
    }
}
