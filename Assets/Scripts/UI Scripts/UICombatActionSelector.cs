using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects.Items;
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
    [SerializeField] private UISelectionPanel SelectionSpellPanel;
    [SerializeField] private UISelectionPanel SelectionItemPanel;
    [SerializeField] private UIPanel selectionDescriptionPanel;
    private PlayerEntityOnBattle associatedPlayerEntityOnBattle;
    private bool hasSecondPanel;

    public void SetPlayerEntity(PlayerEntityOnBattle playerEntityOnBattle)
    {
        associatedPlayerEntityOnBattle = playerEntityOnBattle;
        InitSpells();
        InitItems();
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
        
        SelectionSpellPanel.gameObject.SetActive(false);
        int indexOfSpell = ((PlayerCharacterData)associatedPlayerEntityOnBattle.unitData).getIndexOfSpell(spell);
        BattleManager.NeedToSelectSpellTarget(true, indexOfSpell);
        battleActionSelectionPanel.UIButtons[2].Button.Select();
        Debug.Log($"Selected {spell.Name}");
    }
    
    private void SelectedItem(ItemSO item)
    {
        battleActionSelectionPanel.gameObject.SetActive(true);
        battleActionSelectionPanel2.gameObject.SetActive(true);
        
        SelectionItemPanel.gameObject.SetActive(false);
        int indexOfItem = associatedPlayerEntityOnBattle.Inventory.getIndexOfItem(item);
        BattleManager.NeedToSelectItemTarget(true, indexOfItem);
        battleActionSelectionPanel.UIButtons[3].Button.Select();
        Debug.Log($"Selected {item.Name}");
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
        SelectionSpellPanel.SetSelectionOptions(selectionOptions);
        SelectionSpellPanel.UpdateSelectionOptions();
    }

    private bool InitItems()
    {
        if (associatedPlayerEntityOnBattle.Inventory == null) return false;
        var items = associatedPlayerEntityOnBattle.Inventory.Items;
        if (items == null) return false;
        List<UISelectionPanel.SelectionOption> selectionOptions = new List<UISelectionPanel.SelectionOption>();
        foreach (var item in items)
        {
            selectionOptions.Add(new UISelectionPanel.SelectionOption(item.Item.Name,
                () => { SelectedItem(item.Item); }));
            Debug.Log(item.Item);
        }
        SelectionItemPanel.SetSelectionOptions(selectionOptions);
        SelectionItemPanel.UpdateSelectionOptions();
        return true;
    }

    public void PrintSpells()
    {
        if (SelectionSpellPanel.UIButtons.Count <= 0)
        {
            BattleManager.AbortSpellSelection();
            return;
        }
        battleActionSelectionPanel.gameObject.SetActive(false);
        battleActionSelectionPanel2.gameObject.SetActive(false);
        
        SelectionSpellPanel.gameObject.SetActive(true);
        
        SelectionSpellPanel.UIButtons[0].Button.Select();
    }
    
    public void PrintItems()
    {
        if (!InitItems() || SelectionItemPanel.UIButtons.Count <= 0)
        {
            BattleManager.AbortItemSelection();
            return;
        }
        if (SelectionItemPanel.UIButtons.Count <= 0) return;
        battleActionSelectionPanel.gameObject.SetActive(false);
        battleActionSelectionPanel2.gameObject.SetActive(false);
        
        SelectionItemPanel.gameObject.SetActive(true);
        
        SelectionItemPanel.UIButtons[0].Button.Select();
    }
}
