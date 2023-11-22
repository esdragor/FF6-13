using System;
using System.Collections.Generic;
using UnityEngine;

public class ControlsDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject blurObject;
    [SerializeField] private GameObject panel;

    [SerializeField] private Transform layout;
    [SerializeField] private UIRefs controlsText;
    [SerializeField] private GameObject separatorPrefab;
    
    [Serializable]
    private struct Control
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField,TextArea(1,2)] public string Text { get; private set; }
    }
    
    [SerializeField] private List<Control> controls; // TODO - auto generate this list from input manager
    private void Start()
    {
        for (var index = 0; index < controls.Count; index++)
        {
            var control = controls[index];
            var controlObject = Instantiate(controlsText, layout);
            controlObject.TextMeshProUGUI.text = control.Text;
            controlObject.Image.sprite = control.Sprite;
            
            if(index != controls.Count-1) Instantiate(separatorPrefab, layout);
        }
        separatorPrefab.SetActive(false);
        
        Hide();
    }

    [ContextMenu("Show")]
    public void Show()
    {
        blurObject.SetActive(true);
        panel.SetActive(true);
        
        // TODO - should pause game
    }

    [ContextMenu("Hide")]
    public void Hide()
    {
        blurObject.SetActive(false);
        panel.SetActive(false);
        
        // TODO - should unpause game
    }
}
