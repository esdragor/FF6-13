using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects.Items;
using Scriptable_Objects.Unit;
using UnityEngine;

public class Inventory
{
    public List<InventoryItems> Items { get; private set; }

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
}

public class InventoryItems
{
    public UsableItemSo Item { get; set; }
    public int Quantity { get; set; }
}

public class PlayerController : BattleController
{
    public static event Action<Entity> OnPlayerSpawned;
    public List<Inventory> inventoryItems {get; private set;}
    
    [SerializeField] private List<PlayerCharactersSO> companionsSo = new ();
    

    private Inventory initInvenory(PlayerCharactersSO data)
    {
        Inventory inventory = new Inventory();
        foreach (var item in data.Inventory)
        {
            inventory.AddItem(item, 1);
        }
        return inventory;
    }
    
    protected override void InstantiateEntity()
    {
        base.InstantiateEntity();
        OnPlayerSpawned?.Invoke(entity);
        (entity as PlayerEntity)?.InitPlayer(this);
        
        GameObject go = new GameObject();
        go.transform.SetParent(transform);
        //create here companions
        companions = new List<PlayerEntity>();
        inventoryItems = new List<Inventory>();
        
        inventoryItems.Add(initInvenory(entity.SO as PlayerCharactersSO));
        foreach (var companion in companionsSo)
        {
           
            var companionEntity = go.AddComponent<PlayerEntity>();
            companionEntity.AssignSO(companion);
            companionEntity.InitPlayer(this);
            companionEntity.gameObject.SetActive(false);
            companions.Add(companionEntity);
            inventoryItems.Add(initInvenory(companion));
        }
        go.SetActive(false);
    }
    
    protected override void OnEnable()
    {
        InputManager.OnStartedToTurn += TurnEntity;
        //InputManager.OnStoppedToTurn += TurnEntity;
        InputManager.OnVectorDIrection += MoveEntity;
        
        //InputManager.OnMovingX += MoveEntityX;
        //InputManager.OnMovingY += MoveEntityY;
    }

    protected override void OnDisable()
    {
        InputManager.OnStartedToTurn -= TurnEntity;
        //InputManager.OnStoppedToTurn -= TurnEntity;
        InputManager.OnVectorDIrection -= MoveEntity;
        
        //InputManager.OnMovingX -= MoveEntityX;
        //InputManager.OnMovingY -= MoveEntityY;
    }
    
    public new PlayerEntity getEntity()
    {
        return entity as PlayerEntity;
    }
}
