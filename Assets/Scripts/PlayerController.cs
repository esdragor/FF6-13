using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : InputController
{
    
    protected override void OnEnable()
    {
        InputManager.OnStartedToTurn += TurnEntity;
        //InputManager.OnStoppedToTurn += TurnEntity;
        InputManager.OnMovindDirection += MoveEntity;
    }
    
    protected override void OnDisable()
    {
        InputManager.OnStartedToTurn -= TurnEntity;
        //InputManager.OnStoppedToTurn -= TurnEntity;
        InputManager.OnMovindDirection -= MoveEntity;
    }
}
