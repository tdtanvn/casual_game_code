using OneB;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ShopData
{
    private ItemDatabase database => GameManager.Instance.itemDatabase;
    public List<ShopSlot> ShopNormalItems { get; private set; }
    public ShopSlot SpecialItem { get; private set; }
    public ShopSlot EpicItem { get; private set; }

    public ShopData() 
    {

    }

    public async Task GetShop()
    {
        var shops = await OnlineManager.Instance.API.Send<Shop>(new GetBlueprintObjectCommand("Shop"));

        ShopNormalItems = new List<ShopSlot>();
        foreach (var item in shops.Items)
        {
            var pack = new List<ShopSlot.Pack>();
            foreach (var p in item.Packs)
                pack.Add(new ShopSlot.Pack { item = database.GetItem(p.ItemId), amount = (uint)p.Amount });

            var slot = new ShopSlot
            {
                id = item.Id,
                type = ShopData.GetShopItemType(item.Name),
                currencyType = database.GetItem(item.Currency),
                price = item.Price,
                Packs = pack,
            };
            
            switch (slot.type)
            {
                case ShopItemType.Epic:
                    EpicItem = slot;
                    break;
                case ShopItemType.Special:
                    SpecialItem = slot;
                    break;
                default:
                    ShopNormalItems.Add(slot);
                    break;
            }
        }
    }

    public async void Buy(string id)
    {
        var input1 = new ShopBuyItemInput();
        input1.ItemId = id;

        var buyInfo = await OnlineManager.Instance.API.Send<ShopBuyItemOutput>(new CallGameScriptCommand<ShopBuyItemInput>("Shop", "BuyItem", input1));

        if (buyInfo.Status == "Done")
        {
            InventoryManager.Instance.FetchInventoryData(buyInfo.Inventory);
        }
        else
        {
            var data = new CommonPopup.PopupData(title: null, description: buyInfo.Status, null, "OK");

            GameManager.Instance.commonPopup.PushPopup(data);
        }
    }

    public static ShopItemType GetShopItemType(string input)
    {
        switch (input)
        {
            case "special":
                return ShopItemType.Special;
            case "epic chest":
                return ShopItemType.Epic;
            default:
                return ShopItemType.Pack;
        }
    }
}

public enum ShopCurrencyType { Gem }
public enum ShopItemType { Pack, Special, Epic }