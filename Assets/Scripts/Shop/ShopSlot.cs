using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSlot : Slot
{
    public string id;
    public ShopItemType type;
    public Item currencyType;
    public int price;
    public List<Pack> Packs;

    public class Pack { public Item item; public uint amount; }
}
