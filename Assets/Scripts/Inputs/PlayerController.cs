using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects.Items;
using Scriptable_Objects.Unit;
using Units;
using UnityEditor;
using UnityEngine;

public class PlayerController : BattleController
{
    public static event Action<Entity> OnPlayerSpawned;
    public static event Action<List<PlayerEntity>> TeamStatusChanged;

    public Inventory inventoryItems { get; private set; }

    [SerializeField] private List<PlayerCharactersSO> companionsSo = new();

    private int nbGils = 0;

    private List<PlayerEntity> team = new();


    private static Inventory InitInventory(PlayerCharactersSO data)
    {
        Inventory inventory = new Inventory();
        foreach (var item in data.Inventory)
        {
            inventory.AddItem(item, 1);
        }

        return inventory;
    }

    public void AddGils(int amount)
    {
        nbGils += amount;
    }

    protected override void InstantiateEntity()
    {
        base.InstantiateEntity();
        OnPlayerSpawned?.Invoke(entity);
        (entity as PlayerEntity)?.InitPlayer(this);
        team = new List<PlayerEntity>();

        GameObject go = new GameObject();
        go.transform.SetParent(transform);
        //create here companions
        companions = new List<PlayerEntity>();
        inventoryItems = InitInventory(entity.SO as PlayerCharactersSO);
        nbGils = 100;

        foreach (var companion in companionsSo)
        {
            var companionEntity = go.AddComponent<PlayerEntity>();
            companionEntity.AssignSO(companion);
            companionEntity.InitPlayer(this);
            companionEntity.gameObject.SetActive(false);
            companions.Add(companionEntity);
            inventoryItems.AddItems(InitInventory(companion));
        }

        team.Add(entity as PlayerEntity);
        team.AddRange(companions);
        TeamStatusChanged?.Invoke(team);

        go.SetActive(false);
    }

    protected override void OnEnable()
    {
        InputManager.OnStartedToTurn += TurnEntity;
        InputManager.OnVectorDIrection += MoveEntity;
    }

    protected override void OnDisable()
    {
        InputManager.OnStartedToTurn -= TurnEntity;
        InputManager.OnVectorDIrection -= MoveEntity;
    }

    public new PlayerEntity getEntity()
    {
        return entity as PlayerEntity;
    }

    public void UpdateInventory(Inventory inventory)
    {
        inventoryItems = new Inventory();
        inventoryItems.AddItems(inventory);
    }

    public void UpdateTeam()
    {
        TeamStatusChanged?.Invoke(team);
    }

    public void UseItemIndexOnEntity(int index, Entity target)
    {
        UseItemOnEntity(inventoryItems.Items[index].Item, target);
    }

    public void UseItemOnEntity(UsableItemSo item, Entity target)
    {
        if (inventoryItems.HasItem(item))
        {
            inventoryItems.Use(item, new List<Entity> { target }, entity.unitData);
        }
    }

    public string AddXP(int amount)
    {
        string result = "";

        foreach (var entity in team)
        {
            if (((PlayerCharacterData)entity.unitData).GainXp(amount))
            {
                result += $"{entity.SO.Name} leveled up to {entity.unitData.Level}\n";
                if (entity.unitData.CurrentHp > 0)
                    entity.unitData.TakeDamage(-entity.unitData.MaxHp);
            }
        }

        return result;
    }

    // public void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.I))
    //     {
    //         Debug.Log("Open Inventory");
    //        UseItemIndexOnEntity(1, entity);
    //     }
    // }
}