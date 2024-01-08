using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIRefs : MonoBehaviour
{
    [field: SerializeField] public InputActionReference InputActionReference { get; private set; }
    [field: SerializeField] public TextMeshProUGUI TextMeshProUGUI { get; private set; }
    [field: SerializeField] public Image Image { get; private set; }
    
    public void SetText(string text)
    {
        TextMeshProUGUI.text = text;
    }
    
    public void SetSprite(Sprite sprite)
    {
        Image.sprite = sprite;
    }
}
