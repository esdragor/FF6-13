using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects.Items;
using Scriptable_Objects.Unit;
using UnityEngine;

public class PlayerController : BattleController
{
    public static event Action<Entity> OnPlayerSpawned;
    public List<Inventory> inventoryItems { get; private set; }

    [SerializeField] private List<PlayerCharactersSO> companionsSo = new();


    private static Inventory InitInventory(PlayerCharactersSO data)
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

        inventoryItems.Add(InitInventory(entity.SO as PlayerCharactersSO));
        foreach (var companion in companionsSo)
        {
            var companionEntity = go.AddComponent<PlayerEntity>();
            companionEntity.AssignSO(companion);
            companionEntity.InitPlayer(this);
            companionEntity.gameObject.SetActive(false);
            companions.Add(companionEntity);
            inventoryItems.Add(InitInventory(companion));
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

    public void UpdateInventory(Inventory inventory, int i)
    {
        inventoryItems[i] = null;
        inventoryItems[i] = new Inventory();
        foreach (var item in inventory.Items)
        {
            inventoryItems[i].AddItem(item.Item, item.Quantity);
        }
        //inventoryItems[i] = inventory;
    }
}