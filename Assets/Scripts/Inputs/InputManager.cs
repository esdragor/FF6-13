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
    public static event Action<Direction> OnMovingDirection;
    
    public static event Action OnSelect;
    public static event Action<Direction> OnSelection;
    public static event Action OnCancel;

    private Controls _input;


    private void Start()
    {
        _input = new Controls();
        _input.Exploration.Turn.started += Turn;
        _input.Exploration.Turn.performed += Move;
        _input.Exploration.Turn.canceled += Turn;
        _input.Exploration.Turn.canceled += Move;
        _input.Exploration.Talk.started += Talk;

        _input.Battle.Select.started += Select;
        _input.Battle.Cancel.started += Cancel;
        _input.Battle.Selection.started += Selection;
        OnExploration();
    }

    public void OnExploration()
    {
        _input.Exploration.Enable();
        _input.Battle.Disable();
    }

    public void OnBattle()
    {
        _input.Exploration.Disable();
        _input.Battle.Enable();
        _input.Battle.Select.Enable();
        _input.Battle.Cancel.Enable();
        _input.Battle.Selection.Enable();
    }

    private void Talk(InputAction.CallbackContext obj)
    {
        Debug.Log("Talk");
    }

    private Direction CheckEightDirection(InputAction.CallbackContext context)
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

    private Direction checkFourDirection(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        
        switch (value.x)
        {
            case > 0:
                return (Direction.Right);
            case < 0:
                return Direction.Left;
        }
        
        return value.y switch
        {
            > 0 => Direction.Up,
            < 0 => Direction.Down,
            _ => Direction.None
        };
    }

    public void Move(InputAction.CallbackContext context)
    {
        OnMovingDirection?.Invoke(CheckEightDirection(context));
    }

    public void Turn(InputAction.CallbackContext context)
    {
        OnStartedToTurn?.Invoke(CheckEightDirection(context));
    }

    private void Cancel(InputAction.CallbackContext context)
    {
        OnCancel?.Invoke();
    }

    private void Select(InputAction.CallbackContext context)
    {
        OnSelect?.Invoke();
    }

    private void Selection(InputAction.CallbackContext context)
    {
        OnSelection?.Invoke(checkFourDirection(context));
    }
}