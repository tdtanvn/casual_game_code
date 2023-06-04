using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClanPopup : MonoBehaviour
{
    public enum Type { Create, Search }
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TMP_InputField nameIF;
    [SerializeField] private TMP_InputField descriptionIF;
    [SerializeField] private TMP_InputField levelIF;
    [SerializeField] private Button okBtn;

    private string m_Name, m_Description;
    private uint? m_Level;
    private ClanUI clanUI = null;
    public void Init(ClanUI clanUI)
    {
        this.clanUI = clanUI;
    }

    public void PushPopup(Type type)
    {
        okBtn.onClick.RemoveAllListeners();
        okBtn.onClick.AddListener(() => ConfirmBtnOnlick(type));

        title.text = type == Type.Search ? "Search" : "Create";

        this.gameObject.SetActive(true);
    }

    public void PopPopup()
    {
        this.gameObject.SetActive(false);
    }

    public void ConfirmBtnOnlick(Type type)
    {
        try
        {
            ParseData();
        }
        catch (Exception e)
        {
            var data = new CommonPopup.PopupData(title: "CLAN", description: "Error: " + e.Message, null, "OK");
            GameManager.Instance.commonPopup.PushPopup(data);
            return;
        }

        switch (type)
        {
            case Type.Create:
                CreateClan();
                break;
            case Type.Search:
                Search();
                break;
        }

        PopPopup();
    }

    private void ParseData()
    {
        m_Name = nameIF.text;
        m_Description = descriptionIF.text;
        if (uint.TryParse(levelIF.text, out uint level))
        {
            m_Level = level;
        }
        else
        {
            m_Level = null;
        }
    }

    private void CreateClan()
    {
        OnlineManager.Instance.playerDB.Clan.CreateClan(m_Name, m_Description, m_Level);
    }

    private void Search()
    {
        clanUI.SetupUIWithFilter(m_Name, m_Description, m_Level);
    }
}
