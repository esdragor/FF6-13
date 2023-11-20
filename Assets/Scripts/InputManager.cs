using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class InputManager : MonoBehaviour
{
    public static event Action<Direction> OnStartedToTurn;
    public static event Action<Direction> OnFinishToTurn;
    public static event Action<Direction> OnTriggerDirection;
    public static event Action<Direction> OnMovindDirection;

    private Controls _input;


    private void Start()
    {
        _input = new Controls();
        _input.Inputs.Turn.started += Turn;
        _input.Inputs.Turn.performed += MoveForward;
        _input.Inputs.Turn.canceled += Turn;
        _input.Inputs.Talk.started += Talk;
        
        _input.Inputs.Turn.Enable();
        _input.Inputs.Talk.Enable();
        
    }

    private void Talk(InputAction.CallbackContext obj)
    {
        Debug.Log("Talk");
    }


    public void MoveForward(InputAction.CallbackContext context)
    {
        Debug.Log("MoveForward");
    }

    public void Turn(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        if (value.x > 0)
        {
            OnStartedToTurn?.Invoke(Direction.Right);
            Debug.Log("Right");
        }
        else if (value.x < 0)
        {
            OnStartedToTurn?.Invoke(Direction.Left);
            Debug.Log("Left");
        }
        else if (value.y > 0)
        {
            OnStartedToTurn?.Invoke(Direction.Up);
            Debug.Log("Up");
        }
        else if (value.y < 0)
        {
            OnStartedToTurn?.Invoke(Direction.Down);
            Debug.Log("Down");
        }
    }
}