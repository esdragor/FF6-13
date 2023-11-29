using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects.Spells___Effects;
using Units;
using UnityEngine;

public class UICombatActionSelector : MonoBehaviour
{
    public static event Action OnAutoAttackSelected;
    
    [SerializeField] private UISelectionPanel battleActionSelectionPanel;
    [SerializeField] private UISelectionPanel battleActionSelectionPanel2;
    [SerializeField] private int maxSelectionOptions = 4;
    [SerializeField] private UISelectionPanel gridSelectionPanel;
    [SerializeField] private UISelectionPanel fullSelectionPanel;
    [SerializeField] private UIPanel selectionDescriptionPanel;
    private PlayerEntityOnBattle associatedPlayerEntityOnBattle;
    private bool hasSecondPanel;

    public void SetPlayerEntity(PlayerEntityOnBattle playerEntityOnBattle)
    {
        associatedPlayerEntityOnBattle = playerEntityOnBattle;
        InitSpells();
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
    
    public void UpdateSelectableActions(InterBattle actionner)
    {
        var selectableActions = associatedPlayerEntityOnBattle.PlayerCharactersSo.PossibleActions;
        
        var list1 = new List<UISelectionPanel.SelectionOption>();
        var list2 = new List<UISelectionPanel.SelectionOption>();
        
        foreach (var actionBattle in selectableActions)
        {
            var selectionOption = new UISelectionPanel.SelectionOption($"{actionBattle}", actionner.GetAction(actionBattle));
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

    
    private void selectedSpell(SpellSO spell)
    {
        battleActionSelectionPanel.gameObject.SetActive(true);
        battleActionSelectionPanel2.gameObject.SetActive(true);
        
        fullSelectionPanel.gameObject.SetActive(false);
        int indexOfSpell = ((PlayerCharacterData)associatedPlayerEntityOnBattle.unitData).getIndexOfSpell(spell);
        BattleManager.NeedToSelectSpellTarget(true, indexOfSpell);
        battleActionSelectionPanel.UIButtons[2].Button.Select();
        Debug.Log($"Selected {spell.Name} Bitch");
    }
    
    private void InitSpells()
    {
        var spells = (associatedPlayerEntityOnBattle.unitData as PlayerCharacterData)?.getAllSpells();
        
        List<UISelectionPanel.SelectionOption> selectionOptions = new List<UISelectionPanel.SelectionOption>();
        foreach (var spell in spells)
        {
            selectionOptions.Add(new UISelectionPanel.SelectionOption(spell.Name,
                () => { selectedSpell(spell); }));
            Debug.Log(spell);
        }
        fullSelectionPanel.SetSelectionOptions(selectionOptions);
        fullSelectionPanel.UpdateSelectionOptions();
    }

    public void PrintSpells()
    {
        battleActionSelectionPanel.gameObject.SetActive(false);
        battleActionSelectionPanel2.gameObject.SetActive(false);
        
        fullSelectionPanel.gameObject.SetActive(true);
        
        fullSelectionPanel.UIButtons[0].Button.Select();
    }
}
