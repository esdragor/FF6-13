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
    public event Action<UIButton> OnButtonSelected;
    public event Action<UIButton> OnButtonDeSelected;

    public void OnSelect(BaseEventData eventData)
    {
        OnButtonSelected?.Invoke(this);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        OnButtonDeSelected?.Invoke(this);
    }
}
