using System;
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
    private static readonly int MovingProperty = Shader.PropertyToID("_Moving");
    
    public void InitPlayer(PlayerController controller)
    {
        clampedPosition = transform.position;
        PlayerCharactersSO so = SO as PlayerCharactersSO;
        if (so != null) unitData = new PlayerCharacterData(so, 1);
        _playerController = controller;

        moving = false;

        AssignSprite();
        
        mat = PlayerCharactersSo.ApplyMaterialToEntity(_spriteRenderer);
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
        
        if (y == wantedPosition.y && wantedDirection == Vector2.zero)
        {
            moving = false;
            return;
        }
        
        wantedPosition.y = clampedPosition.y;
        
        if (wantedDirection.y == 0) return;
        
        clampedPosition.y = (y - y % cellSize);
        
        if (wantedDirection.y < 0)
        {
            mat.SetFloat(DirectionProperty, 0f);
        }
        else if (wantedDirection.y > 0)
        {
            mat.SetFloat(DirectionProperty, 1f);

            clampedPosition.y += cellSize;
        }
        
        wantedPosition.y = y + (wantedDirection.normalized).y * cellSize;
    }

    private void UpdateMove()
    {
        //if (directions.Count <= 0) return;
        var speed = delayToMove * Time.deltaTime;
        
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