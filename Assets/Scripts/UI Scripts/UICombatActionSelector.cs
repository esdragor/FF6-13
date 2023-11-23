using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICombatActionSelector : MonoBehaviour
{
    [SerializeField] private UISelectionPanel battleActionSelectionPanel;
    [SerializeField] private UISelectionPanel battleActionSelectionPanel2;
    [SerializeField] private int maxSelectionOptions = 4;
    [SerializeField] private UISelectionPanel gridSelectionPanel;
    [SerializeField] private UIPanel selectionDescriptionPanel;
    private PlayerEntityOnBattle associatedPlayerEntityOnBattle;
    private bool hasSecondPanel;

    public void SetPlayerEntity(PlayerEntityOnBattle playerEntityOnBattle)
    {
        associatedPlayerEntityOnBattle = playerEntityOnBattle;
    }
    
    public void Cleanup()
    {

    }

    public void Show(bool value)
    {
        battleActionSelectionPanel.gameObject.SetActive(value);
        battleActionSelectionPanel2.gameObject.SetActive(value);
        battleActionSelectionPanel2.transform.SetSiblingIndex(0);
        
        gridSelectionPanel.gameObject.SetActive(false);
        selectionDescriptionPanel.gameObject.SetActive(false);
        
        var count = battleActionSelectionPanel.UIButtons.Count;
        
        Debug.Log($"Has {count} buttons");
        
        var hasButton =  count > 0;
        if(hasButton) battleActionSelectionPanel.UIButtons[0].Button.Select();
    }
    
    public void UpdateSelectableActions()
    {
        var selectableActions = associatedPlayerEntityOnBattle.PlayerCharactersSo.PossibleActions;
        var count = selectableActions.Count;
        
        var list1 = new List<UISelectionPanel.SelectionOption>();
        var list2 = new List<UISelectionPanel.SelectionOption>();
        
        
        
        foreach (var actionBattle in selectableActions)
        {
            var selectionOption = new UISelectionPanel.SelectionOption($"{actionBattle}", () => Debug.Log($"Clicked {actionBattle}({(int)actionBattle})"));
            var list = list1.Count < maxSelectionOptions ? list1 : list2;
            list.Add(selectionOption);
        }
        
        battleActionSelectionPanel.SetSelectionOptions(list1);
        battleActionSelectionPanel.UpdateSelectionOptions();
        battleActionSelectionPanel2.SetSelectionOptions(list2);
        battleActionSelectionPanel2.UpdateSelectionOptions();

        if (list2.Count <= 0) battleActionSelectionPanel2.transform.SetSiblingIndex(0);
    }

    [ContextMenu("Cycle Selection Panel")]
    public void CycleSelectionPanel()
    {
        var buttons = battleActionSelectionPanel2.UIButtons;
        var count = buttons.Count;
        if(count <= 0) return;
        
        transform.GetChild(0).SetSiblingIndex(1);
        var panel = battleActionSelectionPanel;
        if(transform.GetChild(0) == panel.transform) panel = battleActionSelectionPanel2;
        
        panel.UIButtons[0].Button.Select();
    }
}
