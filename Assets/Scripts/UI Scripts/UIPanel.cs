using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mainTextMeshProUGUI;
    [SerializeField] private TextMeshProUGUI nameTextMeshProUGUI;
    [SerializeField] private RectTransform namePanelTr;
    
    public void SetText(string mainText)
    {
        mainTextMeshProUGUI.text = mainText;
    }

    public void SetNameText(string nameText)
    {
        namePanelTr.gameObject.SetActive(!string.IsNullOrEmpty(nameText));
        StartCoroutine(ResizeNamePanelRoutine());
        
        return;
        IEnumerator ResizeNamePanelRoutine()
        {
            nameTextMeshProUGUI.text = nameText;
            nameTextMeshProUGUI.ForceMeshUpdate();
            
            yield return null;
            
            var size = nameTextMeshProUGUI.preferredWidth;
        
            Debug.Log(size);
        
            namePanelTr.sizeDelta = new Vector2(size, namePanelTr.sizeDelta.y);
        }
    }
    
    
}
