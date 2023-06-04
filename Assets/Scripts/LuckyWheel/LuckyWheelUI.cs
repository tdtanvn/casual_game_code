using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LuckyWheelUI : MenuUI<LuckyWheelSlotUI, LuckyWheelSlot>
{
    private LuckyWheelData LuckyWheelData => OnlineManager.Instance.playerDB.LuckyWheel;
    protected override List<LuckyWheelSlot> slots => LuckyWheelData.LuckyWheelItems;
    [SerializeField] protected new List<LuckyWheelSlotUI> UIslots;
    [SerializeField] private Button spinBtn;
    [SerializeField] private TextMeshProUGUI spinText;

    private const float rotationOffset = 22; // ~360/8/2
    [SerializeField] private int numSpinFromRecivedRewardToEnd = 2;
    [SerializeField] private float duration = 4f;
    [SerializeField] private float maxSpinSpeed = 500f;
    private LuckyWheelSlot reward = null;
    private Coroutine spinCoroutine;
    private UnityAction fetchInventoryCallback = null;

    private void Awake()
    {
        spinBtn.onClick.AddListener(Spin);
        spinCoroutine = null;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {

    }

    protected override void SetupUI()
    {
        int i = 0;

        foreach (var uislot in UIslots)
        {
            uislot.SetupUI(slots[i]);
            i++;
            if (i >= slots.Count)
                i = 0;
        }

        SetupSpinCountText();
    }

    public void Spin()
    {
        if (!LuckyWheelData.CanClaim)
        {
            var data = new CommonPopup.PopupData(title: null, description: "Cannot spin, try later", null, "OK");

            GameManager.Instance.commonPopup.PushPopup(data);
            return;
        }

        spinCoroutine = StartCoroutine(DoSpin());
    }

    private IEnumerator DoSpin()
    {
        reward = null;
        fetchInventoryCallback = null;
        viewport.rotation = Quaternion.identity;
        float currentZ = 0;
        //start get reward
        LuckyWheelData.ClaimLuckyWheel(SetReward);

        int numToSpin = Random.Range(LuckyWheelData.MinFullSpins, LuckyWheelData.MaxFullSpins);
        //waiting for reward
        while (reward is null || SpinCount(currentZ) < numToSpin)
        {
            yield return null;
            if (IsPassMaxSpin(currentZ))
            {
                SpinTimeout();
                yield break;
            }

            currentZ += maxSpinSpeed * Time.deltaTime;
            viewport.rotation = Quaternion.Euler(0, 0, currentZ);
        }

        //rotate to the reward
        float timer = 0;
        float startRotation = currentZ;
        float targetRotation = FindStopZ(startRotation, reward);
        targetRotation += Random.Range(-rotationOffset, rotationOffset);
        float startSpeed = maxSpinSpeed;

        while (timer < duration && currentZ < targetRotation)
        {
            float t = timer / duration;
            float smoothT = 1f - Mathf.Pow(1f - t, 2f);
            currentZ = Mathf.Min(currentZ + maxSpinSpeed * Time.deltaTime, Mathf.Lerp(startRotation, targetRotation, smoothT));
            viewport.rotation = Quaternion.Euler(0, 0, currentZ);

            timer += Time.deltaTime;
            yield return null;
        }

        SetupSpinCountText();
        ShowReward();
    }

    private void ShowReward()
    {
        var itemList = new List<InventorySlot>();
        itemList.Add(new InventorySlot(reward.item, reward.amount));

        var data = new CommonPopup.PopupData(title: "REWARD", description: null, itemList, "OK", () =>
        {
            if (fetchInventoryCallback != null)
            {
                fetchInventoryCallback();
                fetchInventoryCallback = null;
            }
        });

        GameManager.Instance.commonPopup.PushPopup(data);
    }

    private void SetReward(LuckyWheelSlot reward, UnityAction cb)
    {
        this.reward = reward;
        this.fetchInventoryCallback = cb;
    }

    private void SpinTimeout()
    {
        StopCoroutine(spinCoroutine);

        var data = new CommonPopup.PopupData(title: null, description: "Network Timeout", null, "OK");

        GameManager.Instance.commonPopup.PushPopup(data);

        reward = null;
    }

    private int SpinCount(float currentZ)
    {
        return Mathf.CeilToInt(currentZ / 360);
    }

    private bool IsPassMinSpin(float currentZ)
    {
        return SpinCount(currentZ) > LuckyWheelData.MinFullSpins;
    }

    private bool IsPassMaxSpin(float currentZ)
    {
        return SpinCount(currentZ) > LuckyWheelData.MaxFullSpins;
    }

    private float FindStopZ(float from, LuckyWheelSlot slot)
    {
        float zOffest = UIslots.Find(uislot => uislot.slot.Equals(slot)).originAngle;
        return (Mathf.CeilToInt(from / 360) + numSpinFromRecivedRewardToEnd) * 360 - zOffest;
    }

    private void SetupSpinCountText()
    {
        spinText.text = LuckyWheelData.DailySpinsCount + "/" + LuckyWheelData.DailyLimit;
    }
}
