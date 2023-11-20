using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputController : MonoBehaviour
{
    [SerializeField] protected Entity EntityPrefab;
    protected Entity entity;

    protected abstract void OnEnable();
    protected abstract void OnDisable();

    protected virtual void InstantiateEntity()
    {
        entity = Instantiate(EntityPrefab.gameObject).GetComponent<Entity>();
    }

    private void Start()
    {
        InstantiateEntity();
    }

    protected void TurnEntity(Direction dir)
    {
        entity.Turn(dir);
    }
    
    protected void MoveEntity(Direction dir)
    {
        entity.Move(dir);
    }
}
