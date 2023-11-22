using System;
using DG.Tweening;
using Scriptable_Objects.Unit;
using Units;
using UnityEngine;

public class PlayerEntity : Entity
{
    public PlayerController _playerController { get; private set; }

    [SerializeField] private float cellSize = 1f;
    [SerializeField] private float toleranceMoving = 0.8f;

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
            unitData = new PlayerCharacterData(so,
                so.GrowthTable, 1);
        _playerController = controller;

        moving = false;

        AssignSprite();
    }

    public void AssignSO(PlayerCharactersSO so)
    {
        SO = so;
    }

    private void Update()
    {
        UpdateWantedPosition();
        UpdateMove();
        if (!mat) return; 
        mat.SetFloat(MovingProperty, moving ? 1.0f : 0.0f);
    }

    private void UpdateWantedPosition()
    {
        var position = transform.position;
        
        moving = true;
        
        if (position == wantedPosition && ForwardDirection == Direction.None)
        {
            moving = false;
            return;
        }
        
        wantedPosition = clampedPosition;
        
        if (ForwardDirection == Direction.None) return;
        
        var targetDir = Vector3.zero;
        clampedPosition.y = (position.y - position.y % cellSize);
        clampedPosition.x = (position.x - position.x % cellSize);
        
        if (ForwardDirection == Direction.Up || ForwardDirection == Direction.UpRight ||
            ForwardDirection == Direction.UpLeft)
        {
            mat.SetFloat(DirectionProperty, 0f);
            
            clampedPosition.y += cellSize;
            
            targetDir.y = 1;
        }

        else if (ForwardDirection == Direction.Down || ForwardDirection == Direction.DownLeft ||
                 ForwardDirection == Direction.DownRight)
        {
            mat.SetFloat(DirectionProperty, 1f);
            
            targetDir.y = -1;
        }

        if (ForwardDirection == Direction.Left || ForwardDirection == Direction.DownLeft ||
            ForwardDirection == Direction.UpLeft)
        {
            mat.SetFloat(DirectionProperty, 2f);
            
            targetDir.x = -1;
        }
        else if (ForwardDirection == Direction.Right || ForwardDirection == Direction.UpRight ||
                 ForwardDirection == Direction.DownRight)
        {
            mat.SetFloat(DirectionProperty, 2f);

            clampedPosition.x += cellSize;
            
            targetDir.x = 1;
        }

        wantedPosition = position + (targetDir.normalized) * cellSize;
    }

    private void UpdateMove()
    {
        var speed = delayToMove * Time.deltaTime;
        
        transform.position = Vector3.MoveTowards(transform.position, wantedPosition, speed);
    }
    
    public void InitData(UnitData data)
    {
        unitData = data;
    }
}