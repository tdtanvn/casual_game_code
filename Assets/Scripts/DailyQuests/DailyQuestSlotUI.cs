using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyQuestSlotUI : MenuSlotUI
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private GameObject progress;
    [SerializeField] private Image progressIcon;
    [SerializeField] private TextMeshProUGUI progressAmount;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI progressSliderText;
    [SerializeField] private GameObject claim;
    [SerializeField] private Image claimIcon;
    [SerializeField] private TextMeshProUGUI claimAmount;
    [SerializeField] private Button selectBtn;
    [SerializeField] private Button claimBtn;
    private DailyQuestRightPanelUI rightPanel;

    public void SetupUI(DailyQuestSlot slot, DailyQuestRightPanelUI rightPanel)
    {
        this.rightPanel = rightPanel;
        title.text = slot.data.Name;
        description.text = slot.data.Description;

        progressIcon.sprite = claimIcon.sprite = slot.rewardItem.icon;
        progressAmount.text = claimAmount.text = slot.data.Reward.Quantity.ToString();

        if (slot.data.CanClaim)
        {
            progress.SetActive(false);
            claim.SetActive(true);
            claimBtn.onClick.AddListener(() => Claim(slot.data.Id));
        }
        else
        {
            progress.SetActive(true);
            claim.SetActive(false);
            progressSlider.minValue = 0;
            progressSlider.maxValue = slot.data.Target;
            progressSlider.value = slot.data.Progress;
            progressSliderText.text = slot.data.Progress + "/" + slot.data.Target;
            claimBtn.interactable = false;
        }

        selectBtn.onClick.AddListener(() => SetupRightPanel(slot));
    }

    private void Claim(string id)
    {
        OnlineManager.Instance.playerDB.DailyQuests.Claim(id);
    }

    private void SetupRightPanel(DailyQuestSlot slot)
    {
        rightPanel.SetupUI(slot.rewardItem.icon, slot.data.Reward.Quantity, slot.data.CanClaim, () => Claim(slot.data.Id));
    }
}
