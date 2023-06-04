using System.Collections.Generic;

public class InboxSlot : Slot
{
    public InboxList.Types.Items data;
    public List<Gift> gifts;

    public class Gift
    {
        public Item item;
        public uint amount;
    }
}
