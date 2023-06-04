using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InboxUI : MenuUI<InboxSlotUI, InboxSlot>
{
    protected override List<InboxSlot> slots => OnlineManager.Instance.playerDB.Inbox.InboxItems;


    protected override void OnEnable()
    {
        base.OnEnable();
        Messenger.OnInboxReceived += SetupUI;
        HashUtil.SaveCurrentHashToPreviousHash(HashUtil.ID.Inbox);
    }

    protected override void OnDisable()
    {
        Messenger.OnInboxReceived -= SetupUI;
    }

    protected override void SetupUI()
    {
        base.SetupUI();

        var current = 0;
        for (current = 0; current < slots.Count; current++)
        {
            UIslots[current].SetupUI(slots[current]);
        }
    }

    public void OnClaimAllClick()
    {
        OnlineManager.Instance.playerDB.Inbox.ClaimAll();
    }
}
