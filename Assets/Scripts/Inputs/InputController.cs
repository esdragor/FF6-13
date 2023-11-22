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
        entity.transform.position = transform.position;
    }

    private void Start()
    {
        InstantiateEntity();
        // entity.AddDirectionToMove(Direction.Up);
        // entity.AddDirectionToMove(Direction.Up);
        // entity.AddDirectionToMove(Direction.Up);
        // entity.AddDirectionToMove(Direction.Right);
    }
    
    public virtual Entity getEntity()
    {
        return entity;
    }
    

}
