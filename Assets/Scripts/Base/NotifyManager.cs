using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifyManager : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardNotif;
    [SerializeField] private GameObject rewardNotif;
    [SerializeField] private GameObject questNotif;
    [SerializeField] private GameObject inboxNotif;

    private void OnEnable()
    {
        SetupUI();
        Messenger.OnSceneChanged += SetupUI;
    }

    private void OnDisable()
    {
        Messenger.OnSceneChanged -= SetupUI;
    }

    private void SetupUI()
    {
        leaderboardNotif.SetActive(HashUtil.IsHashChanged(HashUtil.ID.Leaderboard));
        rewardNotif.SetActive(HashUtil.IsHashChanged(HashUtil.ID.Reward));
        questNotif.SetActive(HashUtil.IsHashChanged(HashUtil.ID.Quest));
        inboxNotif.SetActive(HashUtil.IsHashChanged(HashUtil.ID.Inbox));
    }
}
