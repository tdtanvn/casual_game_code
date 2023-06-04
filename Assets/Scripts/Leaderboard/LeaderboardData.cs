using OneB;
using System.Collections.Generic;
using System.Threading.Tasks;

public class LeaderboardData
{
    public List<LeaderboardSlot> LeaderboardGlobalItems { get; private set; }
    public LeaderboardSlot MyRankGlobal { get; private set; }
    public List<LeaderboardSlot> LeaderboardCountryItems { get; private set; }
    public LeaderboardSlot MyRankCountry { get; private set; }

    private const string Name = "ALL_TIME";
    private const string Country = "VN";

    public LeaderboardData()
    {

    }

    public async Task GetLeaderboardData()
    {
        LeaderboardGlobalItems = await GetLeaderboard();
        MyRankGlobal = await GetMyRank();

        LeaderboardCountryItems = await GetLeaderboard(Country);
        MyRankCountry = await GetMyRank(Country);

        Messenger.Broadcast(Messenger.OnLeaderboardReceived);
        HashUtil.SetHash(HashUtil.Type.Current, HashUtil.ID.Leaderboard, HashUtil.GetSHA1Hash(LeaderboardGlobalItems));
    }

    public void UpdateScore(uint score)
    {
        UpdateScoreInternal(score);
    }

    private async Task<List<LeaderboardSlot>> GetLeaderboard(string country = null)
    {
        var input = new OneB.GetTopLeaderboardInput();
        input.Name = Name;
        if (country != null)
            input.Country = country;
        input.Count = 10;
        var topScore = await OnlineManager.Instance.API.Send<TopLeaderboard>(new GetTopScoreLeaderboardCommand(input));
        var list = new List<LeaderboardSlot>();
        foreach (var item in topScore.Items)
        {
            list.Add(new LeaderboardSlot
            {
                Name = string.IsNullOrWhiteSpace(item.PlayerName) ? item.PlayerId : item.PlayerName,
                Country = item.Country,
                Rank = item.Rank,
                Score = item.Score,
            });
        }

        list.Sort((a, b) => a.Rank.CompareTo(b.Rank));
        
        return list;
    }

    private async Task<LeaderboardSlot> GetMyRank(string country = null)
    {
        var input = new OneB.GetMyRankLeaderboardInput();
        input.Name = Name;
        if (country != null)
            input.Country = country;
        var output = await OnlineManager.Instance.API.Send<GetMyRankLeaderboardOutput>(new GetMyRankLeaderboardCommand(input));

        LeaderboardSlot myRank;
        if (output == null)
        {
            myRank = new LeaderboardSlot
            {
                Name = string.IsNullOrWhiteSpace(OnlineManager.Instance.playerDB.PlayerProfile.PlayerName) ? OnlineManager.Instance.playerDB.PlayerProfile.PlayerId : OnlineManager.Instance.playerDB.PlayerProfile.PlayerName,
                Country = OnlineManager.Instance.playerDB.PlayerProfile.Country,
                Rank = 0,
                Score = 0,
            };
        }
        else
        {
            myRank = new LeaderboardSlot
            {
                Name = string.IsNullOrWhiteSpace(OnlineManager.Instance.playerDB.PlayerProfile.PlayerName) ? OnlineManager.Instance.playerDB.PlayerProfile.PlayerId : OnlineManager.Instance.playerDB.PlayerProfile.PlayerName,
                Country = output.Country,
                Rank = output.Rank,
                Score = output.Score,
            };
        }

        return myRank;
    }

    private async void UpdateScoreInternal(uint score)
    {
        var input = new OneB.UpdateScoreLeaderboardInput();
        input.Items.Add(new OneB.UpdateScoreLeaderboardInput.Types.Items { Name = Name, Country = Country, Score = score, Option = ""});

        var updateScore = await OnlineManager.Instance.API.Send<UpdateScoreLeaderboardOutput>(new UpdateScoreLeaderboardCommand(input));

        await GetLeaderboardData();
    }
}
