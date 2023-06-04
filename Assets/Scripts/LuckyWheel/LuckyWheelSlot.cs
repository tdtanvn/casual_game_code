public class LuckyWheelSlot : Slot
{
    public Item item;
    public uint amount;

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        LuckyWheelSlot other = (LuckyWheelSlot)obj;

        return item.id == other.item.id && amount == other.amount;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
