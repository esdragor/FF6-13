using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIActionBar : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private RectTransform self;
    [field: SerializeField] public Image FillImage { get; private set; }
    [SerializeField] private UIPanel actionPanel;
    [Header("Settings")]
    [SerializeField] private Color completeColor = Color.yellow;
    [SerializeField] private Color incompleteColor = Color.gray;
    
    public void SetFill(float fill)
    {
        FillImage.fillAmount = fill;
        FillImage.color = fill >= 1 ? completeColor : incompleteColor;
    }

    public void SetAction(string text,int size)
    {
        actionPanel.SetText(text);
        actionPanel.Self.sizeDelta = new Vector2(size * self.sizeDelta.x, actionPanel.Self.sizeDelta.y);
    }
    
    public void ShowAction(bool show)
    {
        actionPanel.gameObject.SetActive(show);
    }
}
