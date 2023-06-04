using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommonPopupItem : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountTxt;

    public void SetupUI(InventorySlot slot)
    {
        gameObject.SetActive(true);
        icon.sprite = slot.item.icon;
        amountTxt.text = slot.amount.ToString("N0");
    }
}
