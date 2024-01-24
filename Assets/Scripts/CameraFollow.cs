using System;
using System.Collections.Generic;
using Narative;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset;

    [SerializeField] private Transform target;
    [SerializeField] private bool followPlayer = true;
    
    private bool isBlockedUp = false;
    private bool isBlockedDown = false;
    private bool isBlockedLeft = false;
    private bool isBlockedRight = false;

    private void Awake()
    {
        if (followPlayer)
            PlayerController.OnPlayerSpawned += SetTarget;
    }
    
    private void OnEnable()
    {
        BoxMapCollision.CameraStop += StopCamera;
        BoxMapCollision.CameraStart += StartCamera;
        
        StoryInteractor.MoveCameraTO += TPCamTo;
    }
    
    private void OnDisable()
    {
        BoxMapCollision.CameraStop -= StopCamera;
        BoxMapCollision.CameraStart -= StartCamera;
        
        StoryInteractor.MoveCameraTO -= TPCamTo;
    }
    
    private void TPCamTo(Vector2 position, List<Direction> directions)
    {
        transform.position = new Vector3(position.x, position.y, transform.position.z);
        
        foreach (var direction in directions)
        {
            switch (direction)
            {
                case Direction.Up:
                    isBlockedUp = true;
                    break;
                case Direction.Down:
                    isBlockedDown = true;
                    break;
                case Direction.Left:
                    isBlockedLeft = true;
                    break;
                case Direction.Right:
                    isBlockedRight = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
    
    private void StopCamera(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                isBlockedUp = true;
                break;
            case Direction.Down:
                isBlockedDown = true;
                break;
            case Direction.Left:
                isBlockedLeft = true;
                break;
            case Direction.Right:
                isBlockedRight = true;
                break;
        }
    }
    
    private void StartCamera(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                isBlockedUp = false;
                break;
            case Direction.Down:
                isBlockedDown = false;
                break;
            case Direction.Left:
                isBlockedLeft = false;
                break;
            case Direction.Right:
                isBlockedRight = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    private void SetTarget(Entity entity)
    {
        if (target == null)
            target = entity.transform;
    }

    private void LateUpdate()
    {
        if (!target) return;
        
        Vector3 desiredPosition = target.position + offset;
        
        if (isBlockedDown && desiredPosition.y < transform.position.y)
            desiredPosition.y = transform.position.y;
        if (isBlockedUp && desiredPosition.y > transform.position.y)
            desiredPosition.y = transform.position.y;
        if (isBlockedLeft && desiredPosition.x < transform.position.x)
            desiredPosition.x = transform.position.x;
        if (isBlockedRight && desiredPosition.x > transform.position.x)
            desiredPosition.x = transform.position.x;
        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position,
            desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}