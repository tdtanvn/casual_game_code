using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardSlotUI : MenuSlotUI
{
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private Image Country;
    [SerializeField] private TextMeshProUGUI Score;
    [SerializeField] private GameObject goldMedal;
    [SerializeField] private GameObject silverMedal;
    [SerializeField] private GameObject bronzeMedal;
    [SerializeField] private TextMeshProUGUI rankTxt;
    private LeaderboardSlot slot;

    public void SetupUI(LeaderboardSlot slot, bool isMyRank)
    {
        if (this.slot == slot)
            return;
        this.slot = slot;

        nameTxt.text = slot.Name;
        if (string.IsNullOrEmpty(slot.Country))
        {
            Country.gameObject.SetActive(false);
        }
        else
        {
            Country.sprite = GameManager.Instance.countryConfg.GetCountryFlag(slot.Country);
        }
        Score.text = slot.Score.ToString("N0");

        if (isMyRank)
        {
            rankTxt.text = slot.Rank == 0 ? "-" : slot.Rank.ToString();
        }
        else
        {
            SetupRank(slot.Rank);
        }
    }

    private void SetupRank(uint rank)
    {
        goldMedal.SetActive(false);
        silverMedal.SetActive(false);
        bronzeMedal.SetActive(false);
        rankTxt.gameObject.SetActive(false);

        switch (rank)
        {
            case 1:
                goldMedal.SetActive(true);
                return;
            case 2:
                silverMedal.SetActive(true);
                return;
            case 3:
                bronzeMedal.SetActive(true);
                return;
            default:
                rankTxt.gameObject.SetActive(true);
                rankTxt.text = rank.ToString();
                return;
        }
    }
}
