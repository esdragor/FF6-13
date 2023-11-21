using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerOnBattle : BattleController
{
    protected override void OnEnable()
    {
        
    }

    protected override void OnDisable()
    {
        
    }

    protected override void InstantiateEntity()
    {

    }
    
    public void InitPlayer()
    {
        base.InstantiateEntity();
        //(entity as PlayerEntity)?.InitPlayer(this);
    }
    
    public new PlayerEntityOnBattle getEntity()
    {
        return entity as PlayerEntityOnBattle;
    }
}
