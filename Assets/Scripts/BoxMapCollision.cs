using System;
using UnityEngine;

public class BoxMapCollision : MonoBehaviour
{
    [SerializeField] private Direction direction;
    
    public static event Action<Direction> CameraStop;
    public static event Action<Direction> CameraStart;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerEntity>();
        if (player == null) return;

        Debug.Log("Stop camera on " + direction);
        CameraStop?.Invoke(direction);
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerEntity>();
        if (player == null) return;

        Debug.Log("Start camera on " + direction);
        CameraStart?.Invoke(direction);
    }
}