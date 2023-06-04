public class InventorySlot : Slot
{
    public Item item;
    public uint amount;
    public InventorySlot(Item _item, uint _amount)
    {
        item = _item;
        amount = _amount;
    }
}