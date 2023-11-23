using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISelectionPanel : MonoBehaviour
{
    [SerializeField] private UIPanel panel;
    [SerializeField] private Transform layout;
    [SerializeField] private List<SelectionOption> selectionOptions;
    [SerializeField] private UIButton uiButtonPrefab;
    
    private List<UIButton> uiButtons = new List<UIButton>();
    public IReadOnlyList<UIButton> UIButtons => uiButtons;
    
    [Serializable]
    public struct SelectionOption
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public UnityEvent Callback { get; private set; }
        
        public SelectionOption(string name, Action callback)
        {
            Name = name;
            Callback = new UnityEvent();
            Callback.AddListener(callback.Invoke);
        }
    }

    [ContextMenu("Update Selection Options")]
    public void UpdateSelectionOptions()
    {
        foreach (var uiButton in uiButtons)
        {
            Destroy(uiButton);
        }
        uiButtons.Clear();

        foreach (var selectionOption in selectionOptions)
        {
            var button = Instantiate(uiButtonPrefab, layout);
            button.TextMeshProUGUI.text = selectionOption.Name;
            button.Button.onClick.AddListener(() => selectionOption.Callback.Invoke());
            button.OnButtonSelected += UICursor.SetSelectable;
            uiButtons.Add(button);
        }

        ApplyNavigation();
    }

    public void ApplyNavigation()
    {
        var count = uiButtons.Count;

        if(count <= 1) return;
        
        for (int i = 0; i < count; i++)
        {
            var nav = new Navigation
            {
                mode = Navigation.Mode.Explicit,
            };
            if (i - 1 >= 0) nav.selectOnUp = uiButtons[i - 1].Button;
            if (i + 1 <= count - 1) nav.selectOnDown = uiButtons[i + 1].Button;

            uiButtons[i].Button.navigation = nav;
        }
    }

    public void RemoveNavigation()
    {
        var nav = new Navigation
        {
            mode = Navigation.Mode.None,
        };
        
        foreach (var uiButton in uiButtons)
        {
            uiButton.Button.navigation = nav;
        }
    }

    public void SetSelectionOptions(IReadOnlyList<SelectionOption> options) 
    {
        selectionOptions = new List<SelectionOption>(options);
    }

    public void Test()
    {
        Debug.Log("Test");
    }

    private void Awake()
    {
        BattleManager.OnStartSelectionUI += ApplyNavigation;
        BattleManager.OnEndSelectionUI += RemoveNavigation;
    }
}
