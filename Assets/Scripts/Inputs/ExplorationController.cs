using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExplorationController : InputController
{
    protected void TurnEntity(Direction dir)
    {
        entity.Turn(dir);
    }
    
    protected void MoveEntity(Direction dir)
    {
        if(entity.ForwardDirection != dir) entity.Turn(dir);
        
        entity.Move();
    }
}
