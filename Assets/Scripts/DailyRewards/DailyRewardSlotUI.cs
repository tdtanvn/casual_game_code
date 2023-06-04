using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DailyRewardSlotUI : MenuSlotUI, IPointerClickHandler
{
    private ItemUIConfig config => GameManager.Instance.itemUIConfg;
    [SerializeField] private Image frame;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amount;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Transform claimed;
    [SerializeField] private Transform focus;
    [SerializeField] private TextMeshProUGUI focusText;

    private bool canClaim = false;
    private RewardSlot reward;
    public void SetupUI(RewardSlot slot, DailyRewardSlotType type, bool isTomorrowItem)
    {
        if (this.reward == slot) return;
        this.reward = slot;

        frame.gameObject.SetActive(true);
        frame.sprite = config.GetDailyRewardSlotFrame(type);
        icon.sprite = slot.item.icon;
        description.text = slot.data.Description;

        if (slot.data.Amount > 1)
        {
            amount.gameObject.SetActive(true);
            amount.text = slot.data.Amount.ToString();
        }
        else
        {
            amount.gameObject.SetActive(false);
        }

        if (slot.data.Claimed)
        {
            claimed.gameObject.SetActive(true);
        }

        canClaim = slot.data.CanClaim;

        if (isTomorrowItem)
        {
            focus.gameObject.SetActive(true);
            focusText.text = "Tomorrow item";
        }
        else if (canClaim)
        {
            focus.gameObject.SetActive(true);
            focusText.text = "CLAIM";
        }
        else
        {
            focus.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canClaim)
        {
            var itemList = new List<InventorySlot>();
            itemList.Add(new InventorySlot(reward.item, reward.data.Amount));

            var data = new CommonPopup.PopupData(title: "REWARD", description: null, itemList, "OK", () =>
            {
                OnlineManager.Instance.playerDB.DailyRewards.ClaimDailyRewards();
            });
            GameManager.Instance.commonPopup.PushPopup(data);
        }
        else        
        {
            var data = new CommonPopup.PopupData(title: null, description: "Cannot claim, try later", null, "OK");

            GameManager.Instance.commonPopup.PushPopup(data);
        }
    }
}
