using System.Collections;
using TMPro;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mainTextMeshProUGUI;
    [field:SerializeField] public RectTransform Self { get; private set; }
    [SerializeField] private TextMeshProUGUI nameTextMeshProUGUI;
    [SerializeField] private RectTransform namePanelTr;
    
    public void SetText(string mainText)
    {
        mainTextMeshProUGUI.text = mainText;
    }

    public void SetNameText(string nameText)
    {
        if(namePanelTr == null) return;
        namePanelTr.gameObject.SetActive(!string.IsNullOrEmpty(nameText));
        StartCoroutine(ResizeNamePanelRoutine());
        
        return;
        IEnumerator ResizeNamePanelRoutine()
        {
            nameTextMeshProUGUI.text = nameText;
            nameTextMeshProUGUI.ForceMeshUpdate();
            
            yield return null;
            
            var size = nameTextMeshProUGUI.preferredWidth;
            
            namePanelTr.sizeDelta = new Vector2(size, namePanelTr.sizeDelta.y);
        }
    }
    
    
}