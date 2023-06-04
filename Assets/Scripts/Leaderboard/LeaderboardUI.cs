using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MenuUI<LeaderboardSlotUI, LeaderboardSlot>
{
    private LeaderboardData LeaderboardData => OnlineManager.Instance.playerDB.Leaderboard;
    protected override List<LeaderboardSlot> slots => currentSlots;
    private List<LeaderboardSlot> currentSlots;
    private LeaderboardSlot myRank;
    [SerializeField] protected LeaderboardSlotUI myRankUI;
    [SerializeField] protected Button globalBtn;
    [SerializeField] protected Button countryBtn;
    [SerializeField] protected Button friendBtn;
    [SerializeField] protected GameObject focus;
    private int currentTab = 0;
    protected override void OnEnable()
    {
        globalBtn.onClick.AddListener(GlobalTabOnclick);
        countryBtn.onClick.AddListener(CountryTabOnclick);
        SetGlobal();
        base.OnEnable();

        Messenger.OnLeaderboardReceived += SetupUI;
        HashUtil.SaveCurrentHashToPreviousHash(HashUtil.ID.Leaderboard);
    }

    protected override void OnDisable()
    {
        globalBtn.onClick.RemoveAllListeners();
        countryBtn.onClick.RemoveAllListeners();
        base.OnDisable();

        Messenger.OnLeaderboardReceived -= SetupUI;
    }

    protected override void SetupUI()
    {
        base.SetupUI();

        myRankUI.SetupUI(myRank, true);

        for (int i = 0; i < slots.Count; i++)
        {
            UIslots[i].SetupUI(slots[i], false);
        }
    }

    private void SetGlobal()
    {
        currentSlots = LeaderboardData.LeaderboardGlobalItems;
        myRank = LeaderboardData.MyRankGlobal;
    }

    private void SetCountry()
    {
        currentSlots = LeaderboardData.LeaderboardCountryItems;
        myRank = LeaderboardData.MyRankCountry;
    }

    private void GlobalTabOnclick()
    {
        if (currentTab == 0)
            return;

        focus.transform.SetParent(globalBtn.transform, false);
        currentTab = 0;
        SetGlobal();
        SetupUI();
    }
    private void CountryTabOnclick()
    {
        if (currentTab == 1)
            return;

        focus.transform.SetParent(countryBtn.transform, false);
        currentTab = 1;
        SetCountry();
        SetupUI();
    }
}
