using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scriptable_Objects.Items;
using Scriptable_Objects.Spells___Effects;
using Units;
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

    public void Use(UsableItemSo item, List<Entity> targets, UnitData caster)
    {
        Debug.Log($"Use {item.Name} on targets");
        
        var effects = item.Effects.ToList();

        // Items can't miss, so we can do all effects
        foreach (var effect in effects)
        {
            // Check if the effects possess a ignore immunity

            foreach (var tgt in targets)
            {
                switch (effect)
                {
                    case AddOrRemoveAlterationSO addOrRemoveAlterationSo:
                        var ignoreAlteration = effects.Where(e => e is IgnoreAlterationSO).Any(e =>
                            ((IgnoreAlterationSO)e).Alteration == addOrRemoveAlterationSo.Alteration &&
                            ((IgnoreAlterationSO)e).IsPositive);

                        tgt.ApplyAlteration(addOrRemoveAlterationSo.Alteration, ignoreAlteration,
                            addOrRemoveAlterationSo.Remove);
                        break;
                    case DamageEffectSO damageEffectSo:
                        var damage = damageEffectSo.Damage;
                        var element = damageEffectSo.Element;
                        var ignoreDefence = effects.Where(e => e is IgnoreBlockEvadeSO)
                            .Any(e => ((IgnoreBlockEvadeSO)e).IgnoreBlock);
                        var isPourcentDamage = !damageEffectSo.FlatValue;
                        tgt.TakeDamage(damage, element, caster, ignoreDefence, isPourcentDamage);
                        break;
                    case MpRegenSO mpRegenSo:
                        tgt.RegenMpDamage(mpRegenSo.Regen, !mpRegenSo.FlatValue);
                        break;
                }
            }
        }
        RemoveItem(item, 1);
    }
}

public class InventoryItems
{
    public UsableItemSo Item { get; set; }
    public int Quantity { get; set; }
}

