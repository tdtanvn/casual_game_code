using System.Collections.Generic;
using UnityEngine;

public class MenuUI<T, K> : MonoBehaviour where T : MenuSlotUI where K: Slot
{
    [SerializeField] protected GameObject slotPrefab;
    [SerializeField] protected Transform viewport;

    protected virtual List<T> UIslots { get; private set; }
    protected virtual List<K> slots { get; }

    protected virtual void OnEnable()
    {
        SetupUI();
    }

    protected virtual void OnDisable()
    {

    }

    protected virtual void SetupUI()
    {
        if (UIslots == null)
            UIslots = new List<T>();

        for (int i = UIslots.Count; i < slots.Count; i++)
            UIslots.Add(CreataSlot());


        for (int i = slots.Count; i < UIslots.Count; i++)
            UIslots[i].gameObject.SetActive(false);
    }

    protected virtual T CreataSlot()
    {
        return Instantiate(slotPrefab, viewport).GetComponent<T>();
    }
}
