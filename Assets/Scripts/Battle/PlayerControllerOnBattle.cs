using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerOnBattle : PlayerController
{
    protected override void InstantiateEntity()
    {

    }
    
    public void InitPlayer()
    {
        base.InstantiateEntity();
        (entity as PlayerEntity)?.InitPlayer(this);
    }
}
