using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatActionSelectionDisplayer : MonoBehaviour
{
    [SerializeField] private UICombatActionSelector combatActionSelectorPrefab;
    [SerializeField] private Transform combatActionSelectorParent;
    
    private Dictionary<PlayerEntityOnBattle,UICombatActionSelector> characterActionSelectors = new ();

    private InterBattle GetActionsMethod;
    
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
        return combatActionSelector;
    }
    
    public void ShowSelector(bool value = true)
    {
        combatActionSelectorParent.gameObject.SetActive(value);
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