using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DailyQuestRightPanelUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amount;
    [SerializeField] private Button claimBtn;

    public void SetupUI(Sprite icon, uint amount, bool canClaim, UnityAction onClick = null)
    {
        this.icon.sprite = icon;
        this.amount.text = amount.ToString();

        if (canClaim)
        {
            claimBtn.onClick.AddListener(onClick);
        }
        else
        {
            claimBtn.interactable = false;
        }
    }
}
