using System;
using UnityEngine;
using UnityEngine.UI;

public class UICursor : MonoBehaviour
{
    [SerializeField] private RectTransform self;
    private static Selectable currentSelectable;

    private static UICursor instance;

    private void Awake()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            return;
        }
        instance = this;
        gameObject.SetActive(false);
        DontDestroyOnLoad(this);
    }

    public static void Hide()
    {
        instance.gameObject.SetActive(false);
    }
    
    public static void SetSelectable(Selectable selectable)
    {
        instance.gameObject.SetActive(true);
        currentSelectable = selectable;
        
        instance.self.SetParent(currentSelectable.transform);
        instance.self.anchoredPosition = Vector2.zero;
    }
    
    public static void SetSelectable(UIButton uiButton)
    {
        SetSelectable(uiButton.Button);
    }
}
