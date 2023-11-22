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
        directions = new List<Direction>();
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
        
        if (x == wantedPosition.x && wantedDirection == Vector2.zero /*&& (ForwardDirection == Direction.None | directions.Count <= 0)*/)
        {
            moving = false;
            return;
        }
        
        wantedPosition.x = clampedPosition.x;
        
        if (wantedDirection.x == 0) return;
        
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

        wantedPosition.x = x + (wantedDirection.normalized).x * cellSize;
    }

    private void UpdateWantedPositionY()
    {
        var y = transform.position.y;
        
        moving = true;
        
        if (y == wantedPosition.y && wantedDirection == Vector2.zero /*&& (ForwardDirection == Direction.None | directions.Count <= 0)*/)
        {
            moving = false;
            return;
        }
        
        wantedPosition.y = clampedPosition.y;
        
        if (wantedDirection.y == 0) return;
        
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
        
        wantedPosition.y = y + (wantedDirection.normalized).y * cellSize;
    }

    private void UpdateMove()
    {
        //if (directions.Count <= 0) return;
        var speed = delayToMove * Time.deltaTime;
        
        transform.position = Vector3.MoveTowards(transform.position, wantedPosition, speed);
        
        return;
        
        float distance = Vector3.Distance(transform.position, wantedPosition);
        if (distance <= toleranceMoving)
        {
            directions.RemoveAt(0);
            ForwardDirection = Direction.None;
            if (directions.Count > 0)
            {
                Turn(directions[0]);
            }
        }
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

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + (Vector3)wantedDirection, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(wantedPosition, 0.1f);
    }
}