using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LuckyWheelSlotUI : MenuSlotUI
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amount;
    public LuckyWheelSlot slot;
    public float originAngle;

    public void SetupUI(LuckyWheelSlot slot)
    {
        if (this.slot == slot)
            return;

        this.slot = slot;
        icon.sprite = slot.item.icon;
        amount.text = slot.amount.ToString();
        originAngle = transform.rotation.eulerAngles.z;
    }
}
