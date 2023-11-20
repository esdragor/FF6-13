using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManagerBattle : MonoBehaviour
{
    public static event Action OnSelect;
    public static event Action OnCancel;
    
    private Controls _input;
    
    // Start is called before the first frame update
    void Start()
    {
        _input = new Controls();
        
        _input.Battle.Select.started += Select;
        _input.Battle.Cancel.started += Cancel;
        
        _input.Battle.Select.Enable();
    }

    private void Cancel(InputAction.CallbackContext obj)
    {
        throw new NotImplementedException();
    }

    private void Select(InputAction.CallbackContext obj)
    {
       OnCancel?.Invoke();
    }
    
}
