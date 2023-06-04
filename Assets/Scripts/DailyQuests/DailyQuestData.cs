using Google.Protobuf.Collections;
using OneB;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DailyQuestData
{
    private ItemDatabase database => GameManager.Instance.itemDatabase;
    public List<DailyQuestSlot> DailyQuestItems { get; private set; }

    public DailyQuestData()
    {
        
    }

    public async Task GetDailyQuests()
    {
        var dailyQuests = await OnlineManager.Instance.API.Send<DailyQuestsOutput>(new CallGameScriptCommand("DailyQuests", "GetList"));

        UpdateQuestsList(dailyQuests.DailyQuests);
        HashUtil.SetHash(HashUtil.Type.Current, HashUtil.ID.Quest, HashUtil.GetSHA1Hash(DailyQuestItems));
    }

    private void UpdateQuestsList(RepeatedField<DailyQuestsOutput.Types.Dailyquests> quests)
    {
        DailyQuestItems = new List<DailyQuestSlot>();
        foreach (var item in quests)
        {
            DailyQuestItems.Add(new DailyQuestSlot { rewardItem = database.GetItem(item.Reward.Id), data = item });
        }
    }

    private async void Update(string id, uint amount)
    {
        var input = new DailyQuestsUpdateQuestInput();

        var progress = new DailyQuestsUpdateQuestInput.Types.QuestProgress();
        progress.Id = id;
        progress.Amount = amount;
        input.QuestProgress.Add(progress);

        var ouput = await OnlineManager.Instance.API.Send<DailyQuestsUpdateQuestOutput>(new CallGameScriptCommand<DailyQuestsUpdateQuestInput>("DailyQuests", "UpdateQuest", input));

        await GetDailyQuests();
    }

    public void Claim(string id)
    {
        Claim(new List<string> { id });
    }

    public async void Claim(List<string> ids)
    {
        var input = new DailyQuestsClaimQuestInput();
        input.Items.AddRange(ids);

        var ouput = await OnlineManager.Instance.API.Send<DailyQuestsUpdateQuestOutput>(new CallGameScriptCommand<DailyQuestsClaimQuestInput>("DailyQuests", "ClaimQuest", input));

        await GetDailyQuests();
    }
}
