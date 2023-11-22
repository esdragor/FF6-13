using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICombatActionSelector : MonoBehaviour
{
    [SerializeField] private UISelectionPanel battleActionSelectionPanel;
    [SerializeField] private UISelectionPanel gridSelectionPanel;
    [SerializeField] private UIPanel selectionDescriptionPanel;
    private PlayerEntityOnBattle associatedPlayerEntityOnBattle;

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
        gridSelectionPanel.gameObject.SetActive(false);
        selectionDescriptionPanel.gameObject.SetActive(false);
    }
    
    public void UpdateSelectableActions()
    {
        var selectableActions = associatedPlayerEntityOnBattle.PlayerCharactersSo.PossibleActions;
        
        var list = new List<UISelectionPanel.SelectionOption>();
        foreach (var actionBattle in selectableActions)
        {
            var selectionOption = new UISelectionPanel.SelectionOption($"{actionBattle}", () => Debug.Log($"Clicked {actionBattle}({(int)actionBattle})"));
            list.Add(selectionOption);
        }
        
        battleActionSelectionPanel.SetSelectionOptions(list);
        battleActionSelectionPanel.UpdateSelectionOptions();
    }
}
