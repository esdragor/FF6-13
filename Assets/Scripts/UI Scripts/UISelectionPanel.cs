using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UISelectionPanel : MonoBehaviour
{
    [SerializeField] private UIPanel panel;
    [SerializeField] private Transform layout;
    [SerializeField] private List<SelectionOption> selectionOptions;
    [SerializeField] private UIButton uiButtonPrefab;
    [SerializeField] private UICursor cursor;
    
    private List<UIButton> uiButtons = new List<UIButton>();
    
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
            button.OnButtonSelected += cursor.SetSelectable;
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
    
}
