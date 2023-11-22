using UnityEngine;
using UnityEngine.UI;

public class UICursor : MonoBehaviour
{
    [SerializeField] private RectTransform self;
    private Selectable currentSelectable;

    public void SetSelectable(Selectable selectable)
    {
        gameObject.SetActive(true);
        currentSelectable = selectable;
        
        self.SetParent(currentSelectable.transform);
        self.anchoredPosition = Vector2.zero;
    }
    
    public void SetSelectable(UIButton uiButton)
    {
        SetSelectable(uiButton.Button);
    }
}
