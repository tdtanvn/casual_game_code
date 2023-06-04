using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI gemText;

    public void OnEnable()
    {
        SetupUI();
        Messenger.OnInventoryDataReceived += SetupUI;
    }

    public void OnDisable()
    {
        Messenger.OnInventoryDataReceived -= SetupUI;
    }

    private void SetupUI()
    {
        if (energyText != null)
            energyText.text = (InventoryManager.Instance.GetEnergy() ?? 0) + " / 60";

        if (goldText != null) 
            goldText.text = ConvertIntToStringWithCommas(InventoryManager.Instance.GetGold() ?? 0);

        if (gemText != null) 
            gemText.text = ConvertIntToStringWithCommas(InventoryManager.Instance.GetGem() ?? 0);
    }

    private string ConvertIntToStringWithCommas(uint value)
    {
        return value.ToString("N0");
    }

}
