using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    None
}

public class InputManager : MonoBehaviour
{
    public static event Action<Direction> OnStartedToTurn;
    public static event Action<Direction> OnMovindDirection;

    private Controls _input;


    private void Start()
    {
        _input = new Controls();
        _input.Inputs.Turn.started += Turn;
        _input.Inputs.Turn.performed += Move;
        _input.Inputs.Turn.canceled += Turn;
        _input.Inputs.Turn.canceled += Move;
        _input.Inputs.Talk.started += Talk;

        _input.Inputs.Turn.Enable();
        _input.Inputs.Talk.Enable();
    }

    private void Talk(InputAction.CallbackContext obj)
    {
        Debug.Log("Talk");
    }


    public void Move(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();

        Direction dir = value.x > 0 ? Direction.Right :
            (value.x < 0) ? Direction.Left :
            (value.y < 0) ? Direction.Down : (value.y > 0) ? Direction.Up : Direction.None;
        OnMovindDirection?.Invoke(dir);
    }

    public void Turn(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        if (value.x > 0)
            OnStartedToTurn?.Invoke(Direction.Right);
        else if (value.x < 0)
            OnStartedToTurn?.Invoke(Direction.Left);
        else if (value.y > 0)
            OnStartedToTurn?.Invoke(Direction.Up);
        else if (value.y < 0)
            OnStartedToTurn?.Invoke(Direction.Down);
    }
}