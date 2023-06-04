using OneB;
using System.Collections.Generic;

public class GameDB
{
    public GameDB()
    {
        GetItemTable();
    }

    public class ItemInfo
    {
        public string name;
        public ItemType type;
        public ItemCategory category;
    }

    public Dictionary<string, ItemInfo> ItemDict { get; private set; } 
    private ItemTable ItemTable;

    private async void GetItemTable()
    {
        ItemTable = await OnlineManager.Instance.API.Send<ItemTable>(new GetBlueprintObjectCommand("ItemTable"));

        ItemDict = new Dictionary<string, ItemInfo>();

        foreach (var item in ItemTable.Items)
        {
            ItemDict.Add(item.ItemId, new ItemInfo { name = item.Name, type = GetType(item.Type), category = GetCategory(item.Category) });
        }
    }

    public ItemCategory GetItemCategoryFromId(string id)
    {
        if (ItemDict.ContainsKey(id))
            return ItemDict[id].category;
        else
            return ItemCategory.None;
    }

    private ItemType GetType(string type)
    {
        switch (type)
        {
            case "consumable":
                return ItemType.Consumable;
            default:
                return ItemType.NonConsumable;
        }
    }

    private ItemCategory GetCategory(string category)
    {
        switch (category)
        {
            case "Ornament":
                return ItemCategory.Ornament;
            case "Shield":
                return ItemCategory.Shield;
            case "Shoes":
                return ItemCategory.Shoes;
            case "Weapon":
                return ItemCategory.Weapon;
            default:
                return ItemCategory.None;
        }
    }
}
