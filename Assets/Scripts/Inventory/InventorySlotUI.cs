using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    private ItemUIConfig config => GameManager.Instance.itemUIConfg;
    [SerializeField] private Image frame;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amount;

    public void SetupUI(InventorySlot slot = null, ItemCategory category = ItemCategory.None)
    {
        if (slot == null || category == ItemCategory.None)
        {
            frame.gameObject.SetActive(false);
            return;
        }

        frame.gameObject.SetActive(true);
        frame.sprite = config.GetInventoryFrame(category);
        icon.sprite = slot.item.icon;

        if (slot.amount > 1)
        {
            amount.gameObject.SetActive(true);
            amount.text = slot.amount.ToString();
        }
        else
        {
            amount.gameObject.SetActive(false);
        }
    }
}
