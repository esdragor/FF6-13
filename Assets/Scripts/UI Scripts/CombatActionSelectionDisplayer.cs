using System;
using System.Collections.Generic;
using Scriptable_Objects.Spells___Effects;
using UnityEngine;

public class CombatActionSelectionDisplayer : MonoBehaviour
{
    [SerializeField] private UICombatActionSelector combatActionSelectorPrefab;
    [SerializeField] private Transform combatActionSelectorParent;
    
    private Dictionary<PlayerEntityOnBattle,UICombatActionSelector> characterActionSelectors = new ();

    private InterBattle GetActionsMethod;

    private void OnEnable()
    {
        BattleManager.OnCharacterSelected += HideOldSelectorNotCurrent;
        BattleManager.OnShowSpellList += PrintSpells;
    }
    
    private void OnDisable()
    {
        BattleManager.OnCharacterSelected -= HideOldSelectorNotCurrent;
        BattleManager.OnShowSpellList -= PrintSpells;
    }

    private void Cleanup()
    {
        ShowSelector(false);
        
        foreach (var actionSelector in characterActionSelectors.Values)
        {
            actionSelector.Cleanup();
        }
        characterActionSelectors.Clear();
      
    }
   
    public void CreateSelectors(IReadOnlyList<PlayerEntityOnBattle> playerEntitiesOnBattle,InterBattle getActionsMethod)
    {
        GetActionsMethod = getActionsMethod;
        
        Cleanup();
        foreach (var playerEntityOnBattle in playerEntitiesOnBattle)
        {
            characterActionSelectors.Add(playerEntityOnBattle, CreateCombatActionSelector(playerEntityOnBattle));
        }
        ShowSelector();
    }

    public void ShowActionSelector(PlayerEntityOnBattle _,PlayerEntityOnBattle playerEntityOnBattle)
    {
        ShowSelector();
        
        foreach (var actionSelector in characterActionSelectors.Values)
        {
            actionSelector.Show(false);
        }
      
        if(characterActionSelectors.TryGetValue(playerEntityOnBattle, out var characterActionSelector))
        {
            characterActionSelector.UpdateSelectableActions(GetActionsMethod);
            characterActionSelector.Show(true);
            return;
        }
        
        characterActionSelectors.Add(playerEntityOnBattle, CreateCombatActionSelector(playerEntityOnBattle));
    }
    
    private UICombatActionSelector CreateCombatActionSelector(PlayerEntityOnBattle playerEntityOnBattle)
    {
        var combatActionSelector = Instantiate(combatActionSelectorPrefab, combatActionSelectorParent);
        combatActionSelector.name = $"CombatActionSelector {playerEntityOnBattle.SO.name}";
        combatActionSelector.SetPlayerEntity(playerEntityOnBattle);
        combatActionSelector.gameObject.SetActive(false);
        return combatActionSelector;
    }
    
    public void ShowSelector(bool value = true)
    {
        combatActionSelectorParent.gameObject.SetActive(value);
    }
    
    public void HideOldSelectorNotCurrent(PlayerEntityOnBattle old, PlayerEntityOnBattle current)
    {
        characterActionSelectors[old].gameObject.SetActive(false);
        characterActionSelectors[current].gameObject.SetActive(true);
    }
    
    private void PrintSpells(PlayerEntityOnBattle playerEntityOnBattle)
    {
        characterActionSelectors[playerEntityOnBattle].PrintSpells();
    }
    

    public void Hide()
    {
        ShowSelector(false);
    }
}

public interface InterBattle
{
    public Action GetAction(ActionBattle actionBattle);
}

