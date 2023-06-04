using OneB;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class InboxData
{
    private ItemDatabase database => GameManager.Instance.itemDatabase;
    public List<InboxSlot> InboxItems { get; private set; }

    public InboxData() 
    {

    }

    public async Task GetInbox()
    {
        var _inbox = await OnlineManager.Instance.API.Send<InboxList>(new GetInboxListCommand());

        UpdateInbox(_inbox);
        HashUtil.SetHash(HashUtil.Type.Current, HashUtil.ID.Inbox, HashUtil.GetSHA1Hash(InboxItems));
    }

    private void UpdateInbox(InboxList _inbox)
    {
        //if (_inbox.Items[i].Conditions[i])
        //{
        //    //do something
        //}

        InboxItems = new List<InboxSlot>();

        foreach (var item in _inbox.Items)
        {
            if (!item.CanClaim) continue;

            var gifts = new List<InboxSlot.Gift>();
            foreach (var gift in item.Gifts)
            {
                gifts.Add(new InboxSlot.Gift { item = database.GetItem(gift.ItemId), amount = gift.Amount });
            }
            InboxItems.Add(new InboxSlot { gifts = gifts, data = item });
        }

        Messenger.Broadcast(Messenger.OnInboxReceived);
    }

    public void ClaimAsync(List<string> ids)
    {
        var inboxInput = new InboxClaimInput();

        foreach (var item in ids)
        {
            inboxInput.InboxList.Add(item);
        }

        var itemList = new List<InventorySlot>();

        foreach (var item in InboxItems)
        {
            if (ids.Contains(item.data.Id))
            {
                foreach (var gift in item.gifts)
                {
                    itemList.Add(new InventorySlot(gift.item, gift.amount));
                }
            }
        }

        var data = new CommonPopup.PopupData(title: "REWARD", description: null, itemList, "OK", async () =>
        {
            var output = await OnlineManager.Instance.API.Send<InboxClaimOutput>(new ClaimInboxCommand<InboxClaimInput>(inboxInput));

            InventoryManager.Instance.FetchInventoryData(output.Inventory);

            await GetInbox();
        }, "Cancel");

        GameManager.Instance.commonPopup.PushPopup(data);
    }

    public void Claim(string id)
    {
        ClaimAsync(new List<string> { id });
    }

    public void ClaimAll()
    {
        var ids = new List<string>();
        foreach (var item in InboxItems)
        {
            ids.Add(item.data.Id);
        }
        ClaimAsync(ids);
    }
}
