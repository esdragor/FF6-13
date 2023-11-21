using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UITest : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private void OnEnable()
    {
        var root = uiDocument.rootVisualElement;

        var button = root.Q<Button>("Epic");
        
        button.clicked += () => Debug.Log("Clicked");
    }
    
    
}
