using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight,
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
        _input.Exploration.Turn.started += Turn;
        _input.Exploration.Turn.performed += Move;
        _input.Exploration.Turn.canceled += Turn;
        _input.Exploration.Turn.canceled += Move;
        _input.Exploration.Talk.started += Talk;

        _input.Exploration.Turn.Enable();
        _input.Exploration.Talk.Enable();
    }

    private void Talk(InputAction.CallbackContext obj)
    {
        Debug.Log("Talk");
    }

    private Direction CheckDirection(InputAction.CallbackContext context)
    {
        bool isUp = false;
        bool isLeft = false;
        bool nullXvalue = false;
        bool nullYvalue = false;

        var value = context.ReadValue<Vector2>();

        if (value.x == 0 && value.y == 0)
            return (Direction.None);
        if (value.x > 0)
            isLeft = false;
        else if (value.x < 0)
            isLeft = true;
        else
            nullXvalue = true;

        if (value.y > 0)
            isUp = true;
        else if (value.y < 0)
            isUp = false;
        else
            nullYvalue = true;

        switch (isUp)
        {
            case true when isLeft:
                return (Direction.UpLeft);
            case true when !nullXvalue:
                return (Direction.UpRight);
            case false when isLeft && !nullYvalue:
                return (Direction.DownLeft);
            case false when !isLeft && !nullYvalue && !nullXvalue:
                return (Direction.DownRight);
            case true:
                return (Direction.Up);
        }

        if (!nullYvalue)
            return (Direction.Down);
        if (isLeft)
            return (Direction.Left);
        
        return !nullXvalue ? Direction.Right : Direction.None;
    }


    public void Move(InputAction.CallbackContext context)
    {
        OnMovindDirection?.Invoke(CheckDirection(context));
    }

    public void Turn(InputAction.CallbackContext context)
    {
        OnStartedToTurn?.Invoke(CheckDirection(context));
    }

    private void OnDisable()
    {
        _input.Exploration.Turn.Disable();
        _input.Exploration.Talk.Disable();
    }
}