using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyQuestUI : MenuUI<DailyQuestSlotUI, DailyQuestSlot>
{
    protected override List<DailyQuestSlot> slots => OnlineManager.Instance.playerDB.DailyQuests.DailyQuestItems;

    [SerializeField] private DailyQuestRightPanelUI rightPanel;

    protected override void OnEnable()
    {
        base.OnEnable();
        HashUtil.SaveCurrentHashToPreviousHash(HashUtil.ID.Quest);
    }

    protected override void OnDisable()
    {

    }
    protected override void SetupUI()
    {
        base.SetupUI();

        for (int i = 0; i < slots.Count; i++)
        {
            UIslots[i].SetupUI(slots[i], rightPanel);
        }
    }
}
