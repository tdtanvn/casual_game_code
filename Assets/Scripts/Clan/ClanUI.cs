using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClanUI : MenuUI<ClanSlotUI, ClanSlot>
{
    private ClanData ClanData => OnlineManager.Instance.playerDB.Clan;

    [SerializeField] private GameObject inSlotPrefab;
    [SerializeField] private GameObject outSlotPrefab;
    [SerializeField] private ClanRightPanelUI rightPanel;
    [SerializeField] private GameObject popupPrefab;

    private List<ClanSlotUIInState> InSlotsUI;
    private List<ClanSlotUIOutState> OutSlotsUI;
    private List<ClanSlotInState> InSlots;
    private List<ClanSlotOutState> OutSlots;

    private ClanPopup _popup;
    public ClanPopup popup
    {
        get
        {
            if (_popup == null)
            {
                _popup = Instantiate(popupPrefab, this.transform.parent).GetComponent<ClanPopup>();
                _popup.Init(this);
                _popup.gameObject.SetActive(false);
            }

            return _popup;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Messenger.OnClanChanged += SetupUI;
    }

    protected override void OnDisable()
    {
        Messenger.OnClanChanged -= SetupUI;
    }

    protected override void SetupUI()
    {
        if (InSlotsUI != null)
            foreach (var u in InSlotsUI)
                u.gameObject.SetActive(false);

        if (OutSlotsUI != null)
            foreach (var u in OutSlotsUI)
                u.gameObject.SetActive(false);

        if (InSlots == null)
            InSlots = ClanData.clanSlotInStates;
        if (OutSlots == null)
            OutSlots = ClanData.clanSlotOutStates;

        switch (ClanData.currentState)
        {
            case (Clan.State.In):
                if (InSlotsUI == null)
                    InSlotsUI = new List<ClanSlotUIInState>();

                for (int i = InSlotsUI.Count; i < InSlots.Count; i++)
                    InSlotsUI.Add(CreataSlotIn());

                for (int i = 0; i < InSlotsUI.Count; i++)
                    InSlotsUI[i].gameObject.SetActive(i < InSlots.Count);

                for (int i = 0; i < InSlots.Count; i++)
                    InSlotsUI[i].SetupUI(InSlots[i]);

                rightPanel.SetupUI(ClanData.currentClan);
                break;
            case (Clan.State.Out):
                if (OutSlotsUI == null)
                    OutSlotsUI = new List<ClanSlotUIOutState>();

                for (int i = OutSlotsUI.Count; i < OutSlots.Count; i++)
                    OutSlotsUI.Add(CreataSlotOut());

                for (int i = 0; i < OutSlotsUI.Count; i++)
                    OutSlotsUI[i].gameObject.SetActive(i < OutSlots.Count);

                for (int i = 0; i < OutSlots.Count; i++)
                    OutSlotsUI[i].SetupUI(OutSlots[i], rightPanel);

                if (OutSlots.Count > 0)
                    rightPanel.SetupUI(OutSlots[0].clan, popup);
                break;
        }
    }

    public void SetupUIWithFilter(string _name, string _description, uint? _level)
    {
        _name = _name.ToLower(); _description = _description.ToLower();
        switch (ClanData.currentState)
        {
            case Clan.State.In:
                InSlots = ClanData.clanSlotInStates.FindAll(slot => slot.member.id.ToLower().Contains(_name));
                break;
            case Clan.State.Out:
                OutSlots = ClanData.clanSlotOutStates.FindAll(slot => slot.clan.name.ToLower().Contains(_name) && slot.clan.description.ToLower().Contains(_description));
                break;
        }
        SetupUI();
    }

    private ClanSlotUIInState CreataSlotIn()
    {
        return Instantiate(inSlotPrefab, viewport).GetComponent<ClanSlotUIInState>();
    }

    private ClanSlotUIOutState CreataSlotOut()
    {
        return Instantiate(outSlotPrefab, viewport).GetComponent<ClanSlotUIOutState>();
    }

    public void SearchBtnOnlick()
    {
        popup.PushPopup(ClanPopup.Type.Search);
    }
}
