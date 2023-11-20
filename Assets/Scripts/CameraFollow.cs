using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset;
    
    private Transform target;

    private void Start()
    {
        PlayerController.OnPlayerSpawned += SetTarget;
    }

    private void SetTarget(Entity entity)
    {
        target = entity.transform;
    }

    private void LateUpdate()
    {
        if (!target) return;
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, 
            desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        
    }
}
