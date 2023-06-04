using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum DailyRewardSlotType { Normal, Epic }

[CreateAssetMenu(fileName = "New Item UI Setup", menuName = "Inventory/UI Config")]
public class ItemUIConfig : ScriptableObject
{
    [Serializable]
    public class InventoryCategoryFrame
    {
        public ItemCategory category;
        public Sprite sprite;
    }

    [Serializable]
    public class DailyRewardSlotFrame
    {
        public DailyRewardSlotType type;
        public Sprite sprite;
    }

    [SerializeField] private List<InventoryCategoryFrame> inventoryFrames;
    [SerializeField] private List<DailyRewardSlotFrame> dailyRewardSlotFrames;

    public Sprite GetInventoryFrame(ItemCategory category) => inventoryFrames.FirstOrDefault(item => item.category == category).sprite;
    public Sprite GetDailyRewardSlotFrame(DailyRewardSlotType type) => dailyRewardSlotFrames.FirstOrDefault(item => item.type == type).sprite;
}
