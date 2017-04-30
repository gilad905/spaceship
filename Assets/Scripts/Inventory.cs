using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Inventory {
    static readonly List<SsItem> items = new List<SsItem>()
    {
        new SsItem() { Name = "phaser" }
    };

    public static List<SsItem> ownedItems = new List<SsItem>();

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
}
