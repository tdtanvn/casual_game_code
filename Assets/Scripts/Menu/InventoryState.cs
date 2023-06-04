using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryState : MenuState
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform viewport;
    [SerializeField] private Transform focus;

    [Space]
    [Header("Menu Button")]
    [SerializeField] private Button allBtn;
    [SerializeField] private Button weaponBtn;
    [SerializeField] private Button shieldBtn;
    [SerializeField] private Button shoeBtn;
    [SerializeField] private Button ornamentBtn;

    private List<InventorySlotUI> slots;

    private const int InventorySlotCount = 64;

    private int currentSlot = 0;

    private Dictionary<ItemCategory, List<InventorySlot>> Inventory => InventoryManager.Instance.ItemByCategoryDict;
    public void Awake()
    {
        slots = new List<InventorySlotUI>();

        for (int i = 0; i < InventorySlotCount; i++)
        {
            slots.Add(Instantiate(slotPrefab, viewport).GetComponent<InventorySlotUI>());
        }

        SetupInventory(ItemCategory.All);

        allBtn.onClick.AddListener(() => OnTapMenuClick(allBtn.transform, ItemCategory.All));
        weaponBtn.onClick.AddListener(() => OnTapMenuClick(weaponBtn.transform, ItemCategory.Weapon));
        shieldBtn.onClick.AddListener(() => OnTapMenuClick(shieldBtn.transform, ItemCategory.Shield));
        shoeBtn.onClick.AddListener(() => OnTapMenuClick(shoeBtn.transform, ItemCategory.Shoes));
        ornamentBtn.onClick.AddListener(() => OnTapMenuClick(ornamentBtn.transform, ItemCategory.Ornament));
    }

    public void OnTapMenuClick(Transform parent, ItemCategory category)
    {
        SetupInventory(category);
        focus.SetParent(parent, false);
    }

    private void SetupInventory(ItemCategory category)
    {
        currentSlot = 0;

        if (category == ItemCategory.All || category == ItemCategory.Ornament)
        {
            CreateCategorySlots(ItemCategory.Ornament);
        }

        if (category == ItemCategory.All || category == ItemCategory.Shield)
        {
            CreateCategorySlots(ItemCategory.Shield);
        }

        if (category == ItemCategory.All || category == ItemCategory.Shoes)
        {
            CreateCategorySlots(ItemCategory.Shoes);
        }

        if (category == ItemCategory.All || category == ItemCategory.Weapon)
        {
            CreateCategorySlots(ItemCategory.Weapon);
        }

        for (int i = currentSlot; i < InventorySlotCount; i++)
        {
            slots[i].SetupUI();
            slots[i].name = "Slot_Empty";
        }
    }

    private void CreateCategorySlots(ItemCategory category)
    {
        if (Inventory.ContainsKey(category))
        {
            foreach (var item in Inventory[category])
            {
                slots[currentSlot].name = "Slot_" + item.item.name;
                slots[currentSlot].SetupUI(item, category);
                currentSlot++;
            }
        }
    }
}
