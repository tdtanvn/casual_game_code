using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using OneB;
using Google.Protobuf.Collections;

public class ClanData
{
    private PlayerProfile PlayerProfile => OnlineManager.Instance.playerDB.PlayerProfile;
    public List<Clan> ClanItems { get; private set; } = new List<Clan>();
    public Clan currentClan { get; private set; } = null;
    public Clan.State currentState => currentClan == null ? Clan.State.Out : Clan.State.In;
    public List<ClanSlotInState> clanSlotInStates { get; private set; } = new List<ClanSlotInState>();
    public List<ClanSlotOutState> clanSlotOutStates { get; private set; } = new List<ClanSlotOutState>();

    public ClanData()
    {

    }

    public async Task GetClanData()
    {
        var input = new ClanJoinCondition()
        {
            RequiredLevel = 1
        };

        var output = await OnlineManager.Instance.API.Send<ListClanOutput>(new CallGameScriptCommand<ClanJoinCondition>("Clan", "ListClan", input));
        UpdateClanList(output.Clans);
    }

    public async void CreateClan(string name, string description, uint? level)
    {
        var input = new CreateClanInput
        {
            Info = new CreateClanInput.Types.Info
            {
                Name = name,
                Description = description,
                RequiredLevel = level ?? 0
            }
        };

        var output = await OnlineManager.Instance.API.Send<CreateClanOutput>(new CallGameScriptCommand<CreateClanInput>("Clan", "CreateClan", input));
        UpdateCurrentClan(output);
    }

    public async void JoinClan(string id)
    {
        var input = new JoinClanInput
        {
            ClanId = id
        };

        var output = await OnlineManager.Instance.API.Send<JoinClanOutput>(new CallGameScriptCommand<JoinClanInput>("Clan", "JoinClan", input));
        UpdateCurrentClan(output.Clan);

        var data = new CommonPopup.PopupData(title: "CLAN", description: output.Status, null, "OK");
        GameManager.Instance.commonPopup.PushPopup(data);
    }

    public async void LeaveClan(string id)
    {
        var input = new LeaveClanInput
        {
            ClanId = id
        };

        var output = await OnlineManager.Instance.API.Send<LeaveClanOutput>(new CallGameScriptCommand<LeaveClanInput>("Clan", "LeaveClan", input));

        UpdateCurrentClan();

        var data = new CommonPopup.PopupData(title: "CLAN", description: output.Status, null, "OK");
        GameManager.Instance.commonPopup.PushPopup(data);

        await GetClanData();
    }

    private void UpdateClanList(RepeatedField<ClanInfo> list)
    {
        ClanItems.Clear();
        clanSlotOutStates.Clear();

        foreach (var clan in list)
        {
            var c = new Clan(clan);
            ClanItems.Add(c);
            clanSlotOutStates.Add(new ClanSlotOutState { clan = c });

            foreach (var m in c.members)
            {
                if (m.id == PlayerProfile.PlayerId)
                {
                    UpdateCurrentClan(c);
                    break;
                }
            }
        }

        Messenger.Broadcast(Messenger.OnClanChanged);
    }

    private void UpdateCurrentClan(Clan clan)
    {
        currentClan = clan;
        UpdateMemberList();
    }
    private void UpdateCurrentClan(ClanInfo clan)
    {
        UpdateCurrentClan(new Clan(clan));
    }

    private void UpdateCurrentClan(CreateClanOutput clan = null)
    {
        UpdateCurrentClan(new Clan(clan));
    }

    private void UpdateMemberList()
    {
        clanSlotInStates.Clear();
        if (currentClan != null && currentClan.members != null)
            foreach (var m in currentClan.members)
            {
                clanSlotInStates.Add(new ClanSlotInState { member = m });
            }
        else
            currentClan = null;

        Messenger.Broadcast(Messenger.OnClanChanged);
    }
}
