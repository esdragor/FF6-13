using System.Collections.Generic;
using UnityEngine;

public class CombatActionSelectionDisplayer : MonoBehaviour
{
    [SerializeField] private UICombatActionSelector combatActionSelectorPrefab;
    [SerializeField] private Transform combatActionSelectorParent;
    
    private Dictionary<PlayerEntityOnBattle,UICombatActionSelector> characterActionSelectors = new ();
    
    private void Cleanup()
    {
        foreach (var actionSelector in characterActionSelectors.Values)
        {
            actionSelector.Cleanup();
        }
        characterActionSelectors.Clear();
      
    }
   
    public void CreateSelectors(IReadOnlyList<PlayerEntityOnBattle> playerEntitiesOnBattle)
    {
        Cleanup();
        foreach (var playerEntityOnBattle in playerEntitiesOnBattle)
        {
            characterActionSelectors.Add(playerEntityOnBattle, CreateCombatActionSelector(playerEntityOnBattle));
        }
    }

    public void ShowActionSelector(PlayerEntityOnBattle playerEntityOnBattle)
    {
        foreach (var actionSelector in characterActionSelectors.Values)
        {
            actionSelector.Show(false);
        }
      
        if(characterActionSelectors.TryGetValue(playerEntityOnBattle, out var characterActionSelector))
        {
            characterActionSelector.UpdateSelectableActions();
            characterActionSelector.Show(true);
            return;
        }
        
        characterActionSelectors.Add(playerEntityOnBattle, CreateCombatActionSelector(playerEntityOnBattle));
    }
    
    private UICombatActionSelector CreateCombatActionSelector(PlayerEntityOnBattle playerEntityOnBattle)
    {
        var combatActionSelector = Instantiate(combatActionSelectorPrefab, combatActionSelectorParent);
        combatActionSelector.SetPlayerEntity(playerEntityOnBattle);
        return combatActionSelector;
    }
}
