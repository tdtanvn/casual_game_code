using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject dailyRewardPopup;
    public GameObject playerProfilePopup;
    public GameObject inboxPopup;
    public GameObject luckySpinPopup;
    public Slider downloadProgress;

    private static Stack<GameObject> menuStacks;

    private float progress;
    private bool isWaitingForDownload;

    private void Awake()
    {
#if UNITY_EDITOR
        if (OnlineManager.Instance == null || !OnlineManager.Instance.m_Initialzed)
        {
            SceneManager.LoadScene(BuiltInScenes.SplashScene);
        }
#endif

        menuStacks = new Stack<GameObject>();

        AddressableManager.ProgressEvent.AddListener(SetDownloadProgress);
        AddressableManager.CompletionEvent.AddListener(SetDownloadStatus);
    }

    public void DailyRewardPressed()
    {
        Push(dailyRewardPopup);
    }

    public void UserProfilePressed()
    {
        Push(playerProfilePopup);
    }

    public void InboxMessagesPressed()
    {
        Push(inboxPopup);
    }

    public void DailyQuestPressed()
    {
        MenuManager.LoadScene(BuiltInScenes.DailyQuest);
    }

    public void ShopPressed()
    {
        MenuManager.LoadScene(BuiltInScenes.Shop);
    }

    public void InventoryPressed()
    {
        MenuManager.LoadScene(BuiltInScenes.Inventory);
    }

    public void LeaderboardPressed()
    {
        MenuManager.LoadScene(BuiltInScenes.Leaderboard);
    }
    public void ClanPressed()
    {
        MenuManager.LoadScene(BuiltInScenes.Clan);
    }

    public void LuckySpinPressed()
    {
        Push(luckySpinPopup);
    }

    public void BattlePressed()
    {
        AddressableManager.Instance.OnPlayButtionClick();

        if (!AddressableManager.Instance.IsAssetDownloaded())
            StartCoroutine(ShowDownloadProgress());
    }

    private IEnumerator ShowDownloadProgress()
    {
        progress = 0;
        isWaitingForDownload = true;
        downloadProgress.gameObject.SetActive(true);

        while (isWaitingForDownload)
        {
            downloadProgress.value = progress;
            yield return null;
        }


        downloadProgress.gameObject.SetActive(false);
    }

    private void SetDownloadProgress(float progress)
    {
        this.progress = progress;
    }

    private void SetDownloadStatus(bool success)
    {
        isWaitingForDownload = false;

        if (!success)
        {
            var data = new CommonPopup.PopupData(title: "DOWNLOAD", description: $"Download failed", null, "OK");
            GameManager.Instance.commonPopup.PushPopup(data);
        }
    }

    public static void Push(GameObject item)
    {
        menuStacks.Push(item);
        item.SetActive(true);
    }

    public static void Back()
    {
        if (menuStacks.Count > 0)
        {
            menuStacks.Pop().SetActive(false);
        }

        Messenger.Broadcast(Messenger.OnSceneChanged);
    }
}
