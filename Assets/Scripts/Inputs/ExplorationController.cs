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
        entity.Move(dir);
    }
}
