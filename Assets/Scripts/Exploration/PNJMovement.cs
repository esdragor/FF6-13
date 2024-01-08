using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class PNJMovement : Entity
{
    public event Action OnMovementEnded;
    
    [SerializeField] private float cellSize = 0.5f;

    private List<Direction> directions = new List<Direction>();
    private Vector2 wantedDirection = new Vector2();
    private Vector3 clampedPosition;
    private readonly int MovingProperty = Shader.PropertyToID("_Moving");
    private readonly int DirectionProperty = Shader.PropertyToID("_Direction");

    public void AddDirectionToMove(Direction dir)
    {
        directions.Add(dir);
    }
    
    public void ClearDirections()
    {
        directions.Clear();
        wantedDirection = Vector2.zero;
    }

    void Start()
    {
        //Mat = _spriteRenderer.material;
    }

    void OnEnable()
    {
        directions.Clear();
        
        wantedDirection = Vector2.zero;
        clampedPosition = transform.position;
    }

    private void UpdateAnimation()
    {
        if (!mat) return;
        mat.SetFloat(MovingProperty, wantedDirection == Vector2.zero ? 0.0f : 1.0f);
        Turn(InputManager.GetDirection(wantedDirection));
    }

    private void Update()
    {
        if (wantedDirection == Vector2.zero && directions.Count > 0)
        {
            wantedDirection = InputManager.GetVectorDirection(directions[0]);
            directions.RemoveAt(0);
            clampedPosition += new Vector3(wantedDirection.x * cellSize, wantedDirection.y * cellSize, 0f);
        }
        else if (wantedDirection != Vector2.zero)
        {
            var speed = delayToMove * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, clampedPosition, speed);
            var dist = Vector3.Distance(transform.position, clampedPosition);
            // if (dist < cellSize * 0.95f)
            if (transform.position == clampedPosition)
            {
                wantedDirection = Vector2.zero;
                if (directions.Count == 0)
                {
                    OnMovementEnded?.Invoke();
                }
            }
        }

        UpdateAnimation();
    }

    protected override void OnDying()
    {
        Debug.Log("PNJ is dead ?!");
    }

    public override void OnTakeDamage()
    {
        Debug.Log("PNJ take damage ?!");
    }
}