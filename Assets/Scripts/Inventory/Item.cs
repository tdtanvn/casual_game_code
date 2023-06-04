using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Consumable,
    NonConsumable
}

public enum ItemCategory
{
    None,
    Ornament,
    Weapon,
    Shield,
    Shoes,
    All,
}

[CreateAssetMenu(fileName ="New Item", menuName ="Inventory/Item")]
public class Item : ScriptableObject
{
    public string id;
    public ItemType type;
    public Sprite icon;
}
