using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CommonPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Transform itemViewport;
    [SerializeField] private CommonPopupItem itemPrefab;
    [SerializeField] private Button button1;
    [SerializeField] private TextMeshProUGUI button1Txt;
    [SerializeField] private Button button2;
    [SerializeField] private TextMeshProUGUI button2Txt;

    private List<CommonPopupItem> items;

    public class PopupData
    {
        public string title;
        public string description;
        public List<InventorySlot> items;
        public string button1Txt;
        public UnityAction button1onClick;
        public string button2Txt;
        public UnityAction button2onClick;

        public PopupData(string title = null, string description = null, List<InventorySlot> items = null, string button1Txt = null, UnityAction button1onClick = null, string button2Txt = null, UnityAction button2onClick = null)
        {
            this.title = title;
            this.description = description;
            this.items = items;
            this.button1Txt = button1Txt;
            this.button1onClick = button1onClick;
            this.button2Txt = button2Txt;
            this.button2onClick = button2onClick;
        }
    }

    public void PushPopup(PopupData popupData)
    {
        this.gameObject.SetActive(true);

        if (popupData.title == null)
        {
            title.gameObject.SetActive(false);
        }
        else
        {
            title.gameObject.SetActive(true);
            title.text = popupData.title;
        }

        if (popupData.description == null)
        {
            description.gameObject.SetActive(false);
        }
        else
        {
            description.gameObject.SetActive(true);
            description.text = popupData.description;
        }

        if (popupData.items == null)
        {
            itemViewport.gameObject.SetActive(false);
        }
        else
        {
            itemViewport.gameObject.SetActive(true);

            if (items == null)
                items = new List<CommonPopupItem>();

            for (int i = items.Count; i < popupData.items.Count; i++)
                items.Add(Instantiate(itemPrefab, itemViewport).GetComponent<CommonPopupItem>());


            for (int i = 0; i < items.Count; i++)
            {
                if (i < popupData.items.Count)
                {
                    items[i].gameObject.SetActive(true);
                    items[i].SetupUI(popupData.items[i]);
                }
                else
                {
                    items[i].gameObject.SetActive(false);
                }
            }
        }

        if (popupData.button1Txt == null)
        {
            button1.gameObject.SetActive(false);
        }
        else
        {
            button1.gameObject.SetActive(true);
            button1Txt.text = popupData.button1Txt;
            button1.onClick.RemoveAllListeners();
            if (popupData.button1onClick != null)
                button1.onClick.AddListener(popupData.button1onClick);
        }

        button1.onClick.AddListener(Pop);

        if (popupData.button2Txt == null)
        {
            button2.gameObject.SetActive(false);
        }
        else
        {
            button2.gameObject.SetActive(true);
            button2Txt.text = popupData.button2Txt;
            button2.onClick.RemoveAllListeners();
            if (popupData.button2onClick != null)
                button2.onClick.AddListener(popupData.button2onClick);
        }

        button2.onClick.AddListener(Pop);
    }

    public void Pop()
    {
        this.gameObject.SetActive(false);
    }
}


