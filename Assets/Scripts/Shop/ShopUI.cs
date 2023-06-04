using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MenuUI<ShopSlotUI, ShopSlot>
{
    private ShopData ShopData => OnlineManager.Instance.playerDB.Shop;
    protected override List<ShopSlot> slots => ShopData.ShopNormalItems;
    [SerializeField] private ShopEpicSlotUI epicSlotUI;
    [SerializeField] private ShopSpecialSlotUI specialSlotUI;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {

    }

    protected override void SetupUI()
    {
        base.SetupUI();

        var current = 0;
        for (current = 0; current < slots.Count; current++)
        {
            UIslots[current].SetupUI(slots[current]);
        }

        if (ShopData.EpicItem == null)
        {
            epicSlotUI.gameObject.SetActive(false);
        }
        else
        {
            epicSlotUI.SetupUI(ShopData.EpicItem);
        }

        if (ShopData.SpecialItem == null)
        {
            specialSlotUI.gameObject.SetActive(false);
        }
        else
        {
            specialSlotUI.SetupUI(ShopData.SpecialItem);
        }
    }
}
