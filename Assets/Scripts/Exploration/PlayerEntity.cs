using System;
using System.Collections.Generic;
using DG.Tweening;
using Scriptable_Objects.Unit;
using Units;
using UnityEngine;

public class PlayerEntity : Entity
{
    public static event Action OnCinematicStarted;
    public static event Action<bool> OnCinematicEnded;
    
    public PlayerCharactersSO PlayerCharactersSo => (PlayerCharactersSO) SO;
    
    public PlayerController _playerController { get; private set; }

    [SerializeField] private float cellSize = 1f;
    [SerializeField] private float toleranceMoving = 0.8f;

    public Vector2 wantedDirection;
    private Vector3 clampedPosition;
    private Vector3 wantedPosition;
    private bool moving = false;
    private static readonly int DirectionProperty = Shader.PropertyToID("_Direction");
    private static readonly int MovingProperty = Shader.PropertyToID("_Moving");
    
    public void InitPlayer(PlayerController controller)
    {
        clampedPosition = transform.position;
        PlayerCharactersSO so = SO as PlayerCharactersSO;
        if (so != null)
            unitData = new PlayerCharacterData(so, 1);
        _playerController = controller;

        moving = false;

        AssignSprite();
        
        PlayerCharactersSo.ApplyMaterialToEntity(_spriteRenderer);
    }
    
    public void ResetWantedPosition()
    {
        var position = transform.position;
        wantedPosition = position;
        clampedPosition = position;
        wantedDirection = Vector2.zero;
    }

    public void AssignSO(PlayerCharactersSO so)
    {
        SO = so;
    }

    private void Update()
    {
        UpdateWantedPositionX();
        UpdateWantedPositionY();
        
        UpdateMove();
        if (!mat) return; 
        mat.SetFloat(MovingProperty, moving ? 1.0f : 0.0f);
    }

    private void UpdateWantedPositionX()
    {
        var x  = transform.position.x;
        
        moving = true;
        
        if (x == wantedPosition.x && wantedDirection == Vector2.zero)
        {
            moving = false;
            return;
        }
        
        wantedPosition.x = clampedPosition.x;
        
        if (wantedDirection.x == 0) return;
        
        var dirX = wantedDirection.x;
        var raycastHit2D = Physics2D.BoxCast(transform.position, new Vector2(cellSize*0.4f, cellSize*0.4f), 0, new Vector2(dirX, 0), Mathf.Abs(cellSize*0.4f), LayerMask.GetMask("Units", "TileColliders"));
        
        if (raycastHit2D) {
            wantedDirection.x = 0;
            return;
        }
        
        clampedPosition.x = (x - x % cellSize);

        if (wantedDirection.x < 0)
        {
            mat.SetFloat(DirectionProperty, 2f);
        }
        else if (wantedDirection.x > 0)
        {
            mat.SetFloat(DirectionProperty, 2f);

            clampedPosition.x += cellSize;
        }

        var dir = (wantedDirection.normalized).x * cellSize;
        wantedPosition.x = x + dir;

    }

    private void UpdateWantedPositionY()
    {
        var y = transform.position.y;
        
        moving = true;
        
        if (y == wantedPosition.y && wantedDirection == Vector2.zero)
        {
            moving = false;
            return;
        }
        
        wantedPosition.y = clampedPosition.y;
        
        if (wantedDirection.y == 0) return;
        
        var dirY = wantedDirection.y;
        var raycastHit2D = Physics2D.BoxCast(transform.position, new Vector2(cellSize*0.4f, cellSize*0.4f), 0, new Vector2(0, dirY), Mathf.Abs(cellSize*0.4f), LayerMask.GetMask("Units", "TileColliders"));
        
        if (raycastHit2D) {
            wantedDirection.y = 0;
            return;
        }
        
        clampedPosition.y = (y - y % cellSize);
        
        if (wantedDirection.y < 0)
        {
            mat.SetFloat(DirectionProperty, 1f);
        }
        else if (wantedDirection.y > 0)
        {
            mat.SetFloat(DirectionProperty, 0f);

            clampedPosition.y += cellSize;
        }
        
        var dir = (wantedDirection.normalized).y * cellSize;
        wantedPosition.y = y + dir;
    }

    private void UpdateMove()
    {
        //if (directions.Count <= 0) return;
        var speed = delayToMove * Time.deltaTime;
        
        
        // //Check for collisions
        // var raycastHit2D = Physics2D.BoxCast(transform.position, _boxCollider2D.size, 0, new Vector2(0, dir), Mathf.Abs(cellSize*0.4f), LayerMask.GetMask("Units", "TileColliders"));
        //
        // wantedPosition.y = raycastHit2D ? y : y + dir;
        //
        // if (raycastHit2D) {
        //     Debug.Log("Collider was hit on Y");
        // }
        
        //Check for collisions
        // var dirX = wantedDirection.x - transform.position.x;
        // var raycastHit2D = Physics2D.BoxCast(transform.position, _boxCollider2D.size, 0, new Vector2(dirX, 0), Mathf.Abs(cellSize*0.4f), LayerMask.GetMask("Units", "TileColliders"));
        //
        // if (raycastHit2D) {
        //     wantedPosition.x = transform.position.x;
        //     Debug.Log("Collider was hit on X");
        // }
        
        transform.position = Vector3.MoveTowards(transform.position, wantedPosition, speed);
    }

    public void Cinematic(bool started, bool isExplo)
    {
        if(started)
            OnCinematicStarted?.Invoke();
        else
            OnCinematicEnded?.Invoke(isExplo);
    }
    
    public void InitData(UnitData data)
    {
        unitData = data;
    }
}