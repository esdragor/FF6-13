using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatActionBarDisplayer : MonoBehaviour
{
    [Header("Main Action Bar")]
    [SerializeField] private Transform mainActionBarLayout;
    [SerializeField] private UIActionBar actionBarPrefab;
    private Dictionary<PlayerEntityOnBattle,CharacterActionBar> characterActionBars = new ();
    
    private class CharacterActionBar
    { 
        public List<UIActionBar> ActionBars { get; } = new List<UIActionBar>();
        PlayerEntityOnBattle playerEntityOnBattle;
      
        public CharacterActionBar(PlayerEntityOnBattle playerEntityOnBattle)
        {
            this.playerEntityOnBattle = playerEntityOnBattle;
         
            SetupCallbacks();
         
            UpdateActionBars();
        }

        public void SetupCallbacks()
        {
            //TODO: setup callbacks
        }

        public void Cleanup()
        {
            //TODO: remove callbacks
            foreach (var actionBar in ActionBars)
            {
                Destroy(actionBar.gameObject);
            }
            ActionBars.Clear();
        }
      
        public void UpdateActionBars()
        {
            //TODO: update action bars (set fill, set fill color, set action)
        }
      
        public void ShowActionBars(bool show)
        {
            foreach (var actionBar in ActionBars)
            {
                actionBar.gameObject.SetActive(show);
            }
        }
    }
    
    private void ClearActionBars()
    {
        foreach (var characterActionBar in characterActionBars.Values)
        {
            characterActionBar.Cleanup();
        }
        characterActionBars.Clear();
      
    }
   
    public void CreateActionBars(IReadOnlyList<PlayerEntityOnBattle> playerEntitiesOnBattle)
    {
        ClearActionBars();
        foreach (var playerEntityOnBattle in playerEntitiesOnBattle)
        {
            characterActionBars.Add(playerEntityOnBattle, new CharacterActionBar(playerEntityOnBattle));
        }
    }

    public void ShowActionBar(PlayerEntityOnBattle playerEntityOnBattle)
    {
        foreach (var actionBar in characterActionBars.Values)
        {
            actionBar.ShowActionBars(false);
        }
      
        if(characterActionBars.TryGetValue(playerEntityOnBattle, out var characterActionBar))
        {
            characterActionBar.UpdateActionBars();
            return;
        }
      
        characterActionBars.Add(playerEntityOnBattle, new CharacterActionBar(playerEntityOnBattle));
    }
}
