using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButton : MonoBehaviour, ISelectHandler
{
    [field:SerializeField] public Button Button { get; private set; }
    public event Action<UIButton> OnButtonSelected;

    public void OnSelect(BaseEventData eventData)
    {
        OnButtonSelected?.Invoke(this);
    }
}
