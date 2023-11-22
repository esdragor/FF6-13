using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects.Unit;
using UnityEngine;

public class PlayerController : BattleController
{
    public static event Action<Entity> OnPlayerSpawned;
    
    [SerializeField] private List<PlayerCharactersSO> companionsSo = new ();
    
    protected override void InstantiateEntity()
    {
        base.InstantiateEntity();
        OnPlayerSpawned?.Invoke(entity);
        (entity as PlayerEntity)?.InitPlayer(this);
        
        GameObject go = new GameObject();
        go.transform.SetParent(transform);
        //create here companions
        companions = new List<PlayerEntity>();
        foreach (var companion in companionsSo)
        {
           
            var companionEntity = go.AddComponent<PlayerEntity>();
            companionEntity.AssignSO(companion);
            companionEntity.InitPlayer(this);
            companionEntity.gameObject.SetActive(false);
            companions.Add(companionEntity);
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
