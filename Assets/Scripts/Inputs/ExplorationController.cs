using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExplorationController : InputController
{
    protected void TurnEntity(Direction dir)
    {
        entity.Turn(dir);
    }

    private Vector2 targetDir = new Vector2();

    protected void MoveEntityX(float value)
    {
        targetDir.x = value;
        
        //MoveEntity(InputManager.GetDirection(targetDir));
    }
    
    protected void MoveEntityY(float value)
    {
        targetDir.y = value;
        
        //MoveEntity(InputManager.GetDirection(targetDir));
    }
    
    protected void MoveEntity(Vector2 value)
    {
        var playerEntity = entity as PlayerEntity;

        playerEntity.wantedDirection = value;
    }

    protected void MoveEntity(Direction dir)
    {
        var playerEntity = entity as PlayerEntity;
        if (playerEntity == null) return;
        if (playerEntity.directions.Count == 0)
        {
            entity.Turn(dir);
            playerEntity.AddDirectionToMove(dir);
        }
        else if (playerEntity.directions[0] != dir && dir != Direction.None)
        {
            Debug.Log(dir);
            entity.Turn(dir);
            playerEntity.directions.Clear();
            playerEntity.AddDirectionToMove(dir);
        }

        // if(playerEntity.ForwardDirection != dir) entity.Turn(dir);
        //
        // entity.Move();
    }
    
}