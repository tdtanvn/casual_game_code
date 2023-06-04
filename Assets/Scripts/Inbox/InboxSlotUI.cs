using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InboxSlotUI : MenuSlotUI
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI content;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amount;
    [SerializeField] private Button claimBtn;

    private InboxList.Types.Items _data = null;
    public void SetupUI(InboxSlot slot)
    {
        if (slot.data == _data)
            return;

        _data = slot.data;

        title.text = _data.Title; 
        content.text = _data.Content;
        icon.sprite = slot.gifts[0].item.icon;
        amount.text = slot.gifts[0].amount.ToString();

        claimBtn.onClick.AddListener(() => OnlineManager.Instance.playerDB.Inbox.Claim(_data.Id));
    }
}
