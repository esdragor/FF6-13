using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset;

    [SerializeField] private Transform target;
    [SerializeField] private bool followPlayer = true;
    [SerializeField] private Tilemap tilemap;

    private void Awake()
    {
        if (followPlayer)
            PlayerController.OnPlayerSpawned += SetTarget;
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
        Vector3 smoothedPosition = Vector3.Lerp(transform.position,
            desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    private void CheckLastCellSeen()
    {
        Vector3 position = transform.position;
        Vector3Int cellPosition = tilemap.WorldToCell(position);
        Vector3 cellCenterPosition = tilemap.GetCellCenterWorld(cellPosition);
        //int nbCellSeenOnX = 
    }
}