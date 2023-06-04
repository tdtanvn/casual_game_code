using OneB;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDB
{
    public PlayerDB()
    {
        GetPlayerDB();
    }

    private ItemDatabase database => GameManager.Instance.itemDatabase;
    public PlayerProfile PlayerProfile { get; private set; }
    public PlayerData PlayerData { get; private set; }
    public InboxData Inbox { get; private set; }
    public DailyRewardsData DailyRewards { get; private set; }
    public DailyQuestData DailyQuests { get; private set; }
    public LuckyWheelData LuckyWheel { get; private set; }
    public ShopData Shop { get; private set; }
    public LeaderboardData Leaderboard { get; private set; }
    public ClanData Clan { get; private set; }

    private async void GetPlayerDB()
    {
        await GetPlayerInfo();

        List<Task> tasks = new List<Task>();

        Inbox = new InboxData();
        DailyRewards = new DailyRewardsData(async () => await GetPlayerData());
        DailyQuests = new DailyQuestData();
        LuckyWheel = new LuckyWheelData();
        Shop = new ShopData();
        Leaderboard = new LeaderboardData();
        Clan = new ClanData();

        tasks.Add(GetPlayerData());
        tasks.Add(Inbox.GetInbox());
        tasks.Add(DailyRewards.GetDailyRewards());
        tasks.Add(DailyQuests.GetDailyQuests());
        tasks.Add(LuckyWheel.GetLuckyWheelData());
        tasks.Add(Shop.GetShop());
        tasks.Add(Leaderboard.GetLeaderboardData());
        tasks.Add(Clan.GetClanData());

        await Task.WhenAll(tasks);
        //Clan.CreateClan("vjppro", "", 0);
        //Clan.JoinClan("TfYkSz2");
        //Clan.LeaveClan("TfYkSz2");
        Messenger.Broadcast(Messenger.OnInitialized);
    }

    #region PlayerInfo
    private async Task GetPlayerInfo()
    {
        PlayerProfile = await OnlineManager.Instance.API.Send<PlayerProfile>(new GetPlayerObjectCommand("Profile"));

        if (PlayerProfile == null)
        {
            //do something
            return;
        }

        if (PlayerProfile.Ban)
        {
            //do something
        }
    }

    public async void SetPlayerInfo(string name, string country, UnityAction cb)
    {
        PlayerProfileInput input = new PlayerProfileInput();
        input.PlayerName = string.IsNullOrEmpty(name) ? PlayerProfile.PlayerName : name;
        input.Country = string.IsNullOrEmpty(country) ? PlayerProfile.Country : country;

        PlayerProfile = await OnlineManager.Instance.API.Send<PlayerProfile>(new PostPlayerObjectCommand<PlayerProfileInput>("Profile", input));

        cb();
    }
    #endregion
    #region PlayerData
    private async Task GetPlayerData()
    {
        PlayerData = await OnlineManager.Instance.API.Send<PlayerData>(new GetPlayerObjectCommand("Data"));

        InventoryManager.Instance.FetchInventoryData(PlayerData.Inventory);
    }
    #endregion
}
