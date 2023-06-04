using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlotUI : MenuSlotUI, IPointerClickHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI amountTxt;
    [SerializeField] private Image currencyIcon;
    [SerializeField] private TextMeshProUGUI priceTxt;
    private ShopSlot slot;

    public void SetupUI(ShopSlot slot)
    {
        if (this.slot == slot)
            return;

        this.slot = slot;

        itemIcon.sprite = slot.Packs[0].item.icon;
        amountTxt.text = slot.Packs[0].amount.ToString();

        currencyIcon.sprite = slot.currencyType.icon;
        priceTxt.text = slot.price.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var itemList = new List<InventorySlot>();
        foreach (var item in slot.Packs)
        {
            itemList.Add(new InventorySlot(item.item, item.amount));
        }

        var data = new CommonPopup.PopupData(title: "BUY", description: null, itemList, "OK", () =>
        {
            OnlineManager.Instance.playerDB.Shop.Buy(slot.id);
        }, "Cancel");

        GameManager.Instance.commonPopup.PushPopup(data);

    }
}
