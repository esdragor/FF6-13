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
        
        //create here companions
        foreach (var companion in companionsSo)
        {
            var companionEntity = new PlayerEntity();
            companionEntity.AssignSO(companion);
            companionEntity.InitPlayer(this);
            companions.Add(companionEntity);
        }
        
    }
    
    protected override void OnEnable()
    {
        InputManager.OnStartedToTurn += TurnEntity;
        //InputManager.OnStoppedToTurn += TurnEntity;
        InputManager.OnMovingDirection += MoveEntity;
    }

    protected override void OnDisable()
    {
        InputManager.OnStartedToTurn -= TurnEntity;
        //InputManager.OnStoppedToTurn -= TurnEntity;
        InputManager.OnMovingDirection -= MoveEntity;
    }
    
    public new PlayerEntity getEntity()
    {
        return entity as PlayerEntity;
    }
}
