using System;
using System.Collections;
using UnityEngine;

public class DialogueDisplayer : MonoBehaviour
{
    public event Action OnDialogueFullyDisplayed; 
    
    [Header("Components")]
    [SerializeField] private UIPanel topDialogueUiPanel;
    [SerializeField] private UIPanel botDialogueUiPanel;
    
    [Header("Settings")]
    [SerializeField] private float textSpeed = 0.1f;
    [SerializeField] private float baseTextSpeed = 0.1f;
    
    [Header("Debug")]
    [SerializeField] private string debugText;
    [SerializeField] private string debugName;
    
    public bool IsOpen => topDialogueUiPanel.gameObject.activeSelf || botDialogueUiPanel.gameObject.activeSelf;
    public bool IsDisplayingText => currentText != targetText;
    private string currentText;
    private string targetText;
    private Coroutine displayTextCoroutine;
    
    [ContextMenu("Test")]
    private void Test()
    {
        DisplayText(debugText,debugName,true);
    }

    private void Start()
    {
        HidePanel();
    }

    private void OnEnable()
    {
        InputManager.SkipText += ShowTextInstant;
    }
    
    private void OnDisable()
    {
        InputManager.SkipText -= ShowTextInstant;
    }

    [ContextMenu("Hide")]
    public void HidePanel()
    {
        topDialogueUiPanel.gameObject.SetActive(false);
        botDialogueUiPanel.gameObject.SetActive(false);
    }
    
    public void DisplayText(string text, string nameText,bool instant,bool top = false)
    {
        var panel = top ? topDialogueUiPanel : botDialogueUiPanel;
        topDialogueUiPanel.gameObject.SetActive(top);
        botDialogueUiPanel.gameObject.SetActive(!top);
        
        if (displayTextCoroutine != null)
        {
            StopCoroutine(displayTextCoroutine);
        }
        
        currentText = "";
        targetText = text;
        
        panel.SetText(currentText);
        panel.SetNameText(nameText);

        if (instant)
        {
            panel.SetText(targetText);
            OnDialogueFullyDisplayed?.Invoke();
            return;
        }
        
        displayTextCoroutine = StartCoroutine(DisplayTextRoutine(top));
    }
    
    private IEnumerator DisplayTextRoutine(bool useTopPanel)
    {
        var panel = useTopPanel ? topDialogueUiPanel : botDialogueUiPanel;
        
        for (int i = 0; i < targetText.Length; i++)
        {
            currentText = targetText[..i];
            panel.SetText(currentText);
            yield return new WaitForSeconds(textSpeed);
        }
        
        panel.SetText(targetText);
        OnDialogueFullyDisplayed?.Invoke();
    }

    private void ShowTextInstant()
    {
        if (displayTextCoroutine == null) return;
        
        baseTextSpeed = textSpeed;
        textSpeed = 0;
        
        OnDialogueFullyDisplayed += RevertTextSpeed;
    }
    
    private void RevertTextSpeed()
    {
        
        textSpeed = baseTextSpeed;
        OnDialogueFullyDisplayed -= RevertTextSpeed;
    }
}
