using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Google.Protobuf;
using Google.Protobuf.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using OneB;

[DefaultExecutionOrder(-1000)]
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    private ItemDatabase database => GameManager.Instance.itemDatabase;
    public Dictionary<ItemCategory, List<InventorySlot>> ItemByCategoryDict { get; private set; }

    private const string EnergyId = "Energy";
    private const string GoldId = "Gold";
    private const string GemId = "Gem";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    private void CategorizeInventory(List<InventorySlot> slots)
    {
        if (ItemByCategoryDict == null)
            ItemByCategoryDict = new Dictionary<ItemCategory, List<InventorySlot>>();
        else
            ItemByCategoryDict.Clear();

        foreach (var item in slots)
        {
            var category = OnlineManager.Instance.GameDB.GetItemCategoryFromId(item.item.id);
            if (!ItemByCategoryDict.ContainsKey(category))
                ItemByCategoryDict.Add(category, new List<InventorySlot>());
            ItemByCategoryDict[category].Add(item);
        }

        OnlineManager.Instance.playerDB.Leaderboard.UpdateScore(GetGold() ?? 0);
        Messenger.Broadcast(Messenger.OnInventoryDataReceived);
    }

    public void FetchInventoryData(RepeatedField<PlayerData.Types.Inventory> inventory)
    {
        var list = new List<InventorySlot>();

        foreach (var item in inventory)
            list.Add(CreateSlot(item));

        CategorizeInventory(list);
    }
    public void FetchInventoryData(RepeatedField<InboxClaimOutput.Types.Inventory> inventory)
    {
        var list = new List<InventorySlot>();

        foreach (var item in inventory)
            list.Add(CreateSlot(item));

        CategorizeInventory(list);
    }
    public void FetchInventoryData(RepeatedField<ShopBuyItemOutput.Types.Inventory> inventory)
    {
        var list = new List<InventorySlot>();

        foreach (var item in inventory)
            list.Add(CreateSlot(item));

        CategorizeInventory(list);
    }
    public void FetchInventoryData(RepeatedField<LuckyWheelClaimItemOutput.Types.Inventory> inventory)
    {
        var list = new List<InventorySlot>();

        foreach (var item in inventory)
            list.Add(CreateSlot(item));

        CategorizeInventory(list);
    }

    private InventorySlot CreateSlot(PlayerData.Types.Inventory item)
    {
        return new InventorySlot(GetItem(item.ItemId), item.Amount);
    }
    private InventorySlot CreateSlot(InboxClaimOutput.Types.Inventory item)
    {
        return new InventorySlot(GetItem(item.ItemId), item.Amount);
    }
    private InventorySlot CreateSlot(ShopBuyItemOutput.Types.Inventory item)
    {
        return new InventorySlot(GetItem(item.ItemId), item.Amount);
    }
    private InventorySlot CreateSlot(LuckyWheelClaimItemOutput.Types.Inventory item)
    {
        return new InventorySlot(GetItem(item.ItemId), item.Amount);
    }

    private Item GetItem(string itemId)
    {
        return database.GetItem(itemId);
    }

    public uint? GetEnergy() => ItemByCategoryDict[ItemCategory.None].FirstOrDefault(slot => slot.item.id == EnergyId)?.amount;

    public uint? GetGold() => ItemByCategoryDict[ItemCategory.None].FirstOrDefault(slot => slot.item.id == GoldId)?.amount;
    public uint? GetGem() => ItemByCategoryDict[ItemCategory.None].FirstOrDefault(slot => slot.item.id == GemId)?.amount;
}
