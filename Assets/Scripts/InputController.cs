using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputController : MonoBehaviour
{
    [SerializeField] private Entity entity;

    protected abstract void OnEnable();
    protected abstract void OnDisable();

    protected void TurnEntity(Direction dir)
    {
        entity.Turn(dir);
    }
    
    protected void MoveEntity(Direction dir)
    {
        entity.Move(dir);
    }
}
