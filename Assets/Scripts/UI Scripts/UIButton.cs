using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [field:SerializeField] public Button Button { get; private set; }
    [field:SerializeField] public Image Image { get; private set; }
    [field:SerializeField] public TextMeshProUGUI TextMeshProUGUI { get; private set; }
    
    [SerializeField] private Color selectedColor = Color.yellow;
    [SerializeField] private Color deselectedColor = Color.black;
    public event Action<UIButton> OnButtonSelected;
    public event Action<UIButton> OnButtonDeSelected;

    public void OnSelect(BaseEventData eventData)
    {
        TextMeshProUGUI.color = selectedColor;
        OnButtonSelected?.Invoke(this);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        TextMeshProUGUI.color = deselectedColor;
        OnButtonDeSelected?.Invoke(this);
    }
}
