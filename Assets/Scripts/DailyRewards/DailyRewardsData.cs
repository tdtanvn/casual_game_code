using Google.Protobuf.Collections;
using OneB;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class DailyRewardsData
{
    private ItemDatabase database => GameManager.Instance.itemDatabase;
    public List<RewardSlot> DailyRewardsItems { get; private set; }
    private UnityAction claimedDailyRewardCallback;
    public uint currentStep = 0;
    public DailyRewardsData(UnityAction claimedDailyRewardCallback)
    {
        this.claimedDailyRewardCallback = claimedDailyRewardCallback;
    }

    public async Task GetDailyRewards()
    {
        var rewardsList = await OnlineManager.Instance.API.Send<DailyRewardsList>(new CallGameScriptCommand("DailyRewards", "GetList"));
        
        UpdateRewardList(rewardsList);
        HashUtil.SetHash(HashUtil.Type.Current, HashUtil.ID.Reward, HashUtil.GetSHA1Hash(DailyRewardsItems));
    }

    public async void ClaimDailyRewards()
    {
        var rewardsList = await OnlineManager.Instance.API.Send<DailyRewardsList>(new CallGameScriptCommand("DailyRewards", "ClaimRewards"));
        UpdateRewardList(rewardsList);

        //InventoryManager.Instance.FetchInventoryData(rewardsList.Inventory);
        claimedDailyRewardCallback();
    }

    private void UpdateRewardList(DailyRewardsList rewardsList)
    {
        var steps = rewardsList.Steps;

        DailyRewardsItems = new List<RewardSlot>();
        foreach (var item in steps)
        {
            DailyRewardsItems.Add(new RewardSlot { item = database.GetItem(item.ItemId), data = item });
        }

        currentStep = rewardsList.CurStep - 1;

        Messenger.Broadcast(Messenger.OnDailyRewardChanged);
    }
}
