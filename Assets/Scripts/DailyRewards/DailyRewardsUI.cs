using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRewardsUI : MenuUI<DailyRewardSlotUI, RewardSlot>
{
    private DailyRewardsData DailyRewards => OnlineManager.Instance.playerDB.DailyRewards;
    protected override List<RewardSlot> slots => DailyRewards.DailyRewardsItems;

    protected override void OnEnable()
    {
        base.OnEnable();
        Messenger.OnDailyRewardChanged += SetupUI;
        HashUtil.SaveCurrentHashToPreviousHash(HashUtil.ID.Reward);
    }

    protected override void OnDisable()
    {
        Messenger.OnDailyRewardChanged -= SetupUI;
    }

    protected override void SetupUI()
    {
        base.SetupUI();

        for (int i = 0; i < slots.Count; i++)
        {
            bool isTomorrowItem = DailyRewards.currentStep == i && !slots[i].data.CanClaim;
            UIslots[i].SetupUI(slots[i], (i + 1) % 7 == 0 ? DailyRewardSlotType.Epic : DailyRewardSlotType.Normal, isTomorrowItem);
        }
    }
}
