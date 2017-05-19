using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public static List<SsItem> ownedItems = new List<SsItem>();
    public static GameObject Items;

    static List<SsItem> items = null;
    static SsItem currentItem = null;

    private void Start()
    {
        Items itemsCtrl = gameObject.GetComponent<Items>();
        items = itemsCtrl.items;
    }

    public static bool OwnItem(string itemName)
    {
        SsItem item = items.Find(i => i.Name == itemName);
        if (item == null)
            throw new System.Exception("Item doesn't exist with the name: " + itemName);
        else
        {
            if (ownedItems.Contains(item))
                return false;
            else
            {
                ownedItems.Add(item);
                if (ownedItems.Count == 1)
                    currentItem = item;
                return true;
            }
        }
    }

    public static bool DisownItem(string itemName)
    {
        SsItem item = items.Find(i => i.Name == itemName);
        if (item == null)
            throw new System.Exception("Item doesn't exist with the name: " + itemName);
        else
        {
            if (ownedItems.Contains(item))
            {
                ownedItems.Remove(item);
                return true;
            }
            else
                return false;
        }
    }

    public static bool UseCurrentItem()
    {
        if (currentItem != null)
        {
            currentItem.Use();
            return true;
        }
        return false;
    }
}
