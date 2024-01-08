using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects.Items;
using UnityEngine;

public class Inventory
{
    public List<InventoryItems> Items { get; private set; }
    
    public Inventory()
    {
        Items = new List<InventoryItems>();
    }

    public void AddItem(UsableItemSo item, int quantity)
    {
        Items ??= new List<InventoryItems>();
        var itemInInventory = Items.Find(i => i.Item == item);
        if (itemInInventory != null)
        {
            itemInInventory.Quantity += quantity;
        }
        else
        {
            Items.Add(new InventoryItems {Item = item, Quantity = quantity});
        }
    }

    public void RemoveItem(UsableItemSo item, int quantity)
    {
        var itemInInventory = Items.Find(i => i.Item == item);
        if (itemInInventory != null)
        {
            itemInInventory.Quantity -= quantity;
            if (itemInInventory.Quantity <= 0)
            {
                Items.Remove(itemInInventory);
            }
        }
        else
        {
            Debug.Log("Item not found in inventory");
        }
    }
    public bool HasItem(UsableItemSo item)
    {
        var itemInInventory = Items.Find(i => i.Item == item);
        return itemInInventory != null;
    }

    public int getIndexOfItem(ItemSO item)
    {
        var itemInInventory = Items.Find(i => i.Item == item);
        return Items.IndexOf(itemInInventory);
    }

    public void AddItems(Inventory initInventory)
    {
        foreach (var item in initInventory.Items)
        {
            AddItem(item.Item, item.Quantity);
        }
    }
}

public class InventoryItems
{
    public UsableItemSo Item { get; set; }
    public int Quantity { get; set; }
}

