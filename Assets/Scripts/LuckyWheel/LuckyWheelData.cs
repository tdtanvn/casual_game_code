using OneB;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Events;

public class LuckyWheelData
{
    private ItemDatabase database => GameManager.Instance.itemDatabase;
    public List<LuckyWheelSlot> LuckyWheelItems { get; private set; }
    public int DailyLimit { get; private set; }
    public int MinFullSpins { get; private set; }
    public int MaxFullSpins { get; private set; }
    public uint DailySpinsCount { get; private set; }
    public bool CanClaim { get; private set; }
    public LuckyWheelData()
    {

    }

    public async Task GetLuckyWheelData()
    {
        var luckyWheel = await OnlineManager.Instance.API.Send<LuckyWheel>(new CallGameScriptCommand("LuckyWheel", "GetList"));

        var list = new List<LuckyWheelSlot>();

        foreach (var item in luckyWheel.Items)
            list.Add(new LuckyWheelSlot { item = database.GetItem(item.ItemId), amount = (uint)item.Quantity });

        LuckyWheelItems = list;
        DailyLimit = luckyWheel.DailyLimit;
        MinFullSpins = luckyWheel.MinFullSpins;
        MaxFullSpins = luckyWheel.MaxFullSpins;
        DailySpinsCount = 0;
        CanClaim = false;

        CheckCanClaimLuckyWheel();
    }

    public async void CheckCanClaimLuckyWheel()
    {
        var canClaim = await OnlineManager.Instance.API.Send<LuckyWheelCanClaimOutput>(new CallGameScriptCommand("LuckyWheel", "CanClaimItem"));
        CanClaim = canClaim.CanClaim;
    }

    public async void ClaimLuckyWheel(UnityAction<LuckyWheelSlot, UnityAction> cb)
    {
        var claimItem = await OnlineManager.Instance.API.Send<LuckyWheelClaimItemOutput>(new CallGameScriptCommand("LuckyWheel", "ClaimItem"));
        var reward = new LuckyWheelSlot { item = database.GetItem(claimItem.ItemReward.ItemId), amount = claimItem.ItemReward.Quantity };
        
        DailySpinsCount = claimItem.LuckyWheel.DailySpins;
        cb(reward, () =>
        {
            InventoryManager.Instance.FetchInventoryData(claimItem.Inventory);
        });
        CheckCanClaimLuckyWheel();
    }

}