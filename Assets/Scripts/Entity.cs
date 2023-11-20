using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected BoxCollider2D _boxCollider2D;

    public void Turn(Direction dir)
    {
        Debug.Log("Turned " + dir);
    }

    public void Move(Direction dir)
    {
        Debug.Log("Moved " + dir);
        
    }
}