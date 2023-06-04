using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory/Database")]
public class ItemDatabase : ScriptableObject
{    
    public Item[] Items;

    public Item GetItem( string itemId)
    {
        return Items.First(item => item.id == itemId);
    }

#if UNITY_EDITOR
    [ContextMenu("Get Item List")]
    public void GetItemList()
    {
        string folder = "Assets/Items/Item";

        List<Item> items = new List<Item>();
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(Item).Name, new string[] { folder });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Item item = AssetDatabase.LoadAssetAtPath<Item>(path);
            if (item != null)
            {
                items.Add(item);
            }
        }

        Items = items.ToArray();
    }
#endif
}
