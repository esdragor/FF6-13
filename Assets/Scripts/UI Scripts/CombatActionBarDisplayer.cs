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
      
        public CharacterActionBar(PlayerEntityOnBattle playerEntityOnBattle,UIActionBar actionBarPrefab,Transform mainActionBarLayout)
        {
            this.playerEntityOnBattle = playerEntityOnBattle;
         
            SetupCallbacks();

            InstantiateBars(actionBarPrefab,mainActionBarLayout);
         
            UpdateActionBarsPercent(playerEntityOnBattle.PercentageActionBar);
            UpdateActionBarsActions(playerEntityOnBattle.ActionsStack);
        }

        private void SetupCallbacks()
        {
            playerEntityOnBattle.OnActionBarValueChanged += UpdateActionBarsPercent;
            playerEntityOnBattle.OnActonQueueUpdated += UpdateActionBarsActions;
        }
        
        private void InstantiateBars(UIActionBar actionBarPrefab,Transform mainActionBarLayout)
        {
            var count = playerEntityOnBattle.NbBarre;
            
            for (int i = 0; i < count; i++)
            {
                var actionBar = Instantiate(actionBarPrefab, mainActionBarLayout);
                actionBar.gameObject.name = $"Action Bar {{i}} ({playerEntityOnBattle.SO.Name})";
                ActionBars.Add(actionBar);
            }
        }

        public void Cleanup()
        {
            playerEntityOnBattle.OnActionBarValueChanged -= UpdateActionBarsPercent;
            playerEntityOnBattle.OnActonQueueUpdated -= UpdateActionBarsActions;
            
            //TODO: remove callbacks
            foreach (var actionBar in ActionBars)
            {
                Destroy(actionBar.gameObject);
            }
            ActionBars.Clear();
        }
        
        public void UpdateActionBarsActions(IReadOnlyList<ActionToStack> actionsToQueue)
        {
            Debug.Log($"Updating actions ({actionsToQueue.Count})");
            
            foreach (var actionBar in ActionBars)
            {
                actionBar.ShowAction(false);
            }

            var index = 0;
            foreach (var t in actionsToQueue)
            {
                var cost = t.Cost;
                var actionBar = ActionBars[index];
                actionBar.SetAction(t.Name,cost);
                actionBar.ShowAction(true);
                index += cost;
            }
        }
      
        public void UpdateActionBarsPercent(float value)
        {
            var count = ActionBars.Count;
            var percentPerBar = 1f / count;
            
            for (int i = 0; i < count; i++)
            {
                var actionBar = ActionBars[i];
                var percent = value - percentPerBar * i;
                if (percent < 0) percent = 0;
                if (percent > percentPerBar) percent = percentPerBar;
                actionBar.SetFill(percent / percentPerBar);
            }
            
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
        ShowBar(false);
        
        foreach (var characterActionBar in characterActionBars.Values)
        {
            characterActionBar.Cleanup();
        }
        characterActionBars.Clear();
      
    }
   
    public void CreateActionBars(IReadOnlyList<PlayerEntityOnBattle> playerEntitiesOnBattle)
    {
        ClearActionBars();
        Debug.Log($"Creating bars for {playerEntitiesOnBattle.Count}");
        foreach (var playerEntityOnBattle in playerEntitiesOnBattle)
        {
            ShowActionBar(null,playerEntityOnBattle);
        }
        ShowBar(true);
    }

    public void ShowActionBar(PlayerEntityOnBattle _,PlayerEntityOnBattle playerEntityOnBattle)
    {
        ShowBar(true);
        
        foreach (var actionBar in characterActionBars.Values)
        {
            actionBar.ShowActionBars(false);
        }
        
        if(characterActionBars.TryGetValue(playerEntityOnBattle, out var characterActionBar))
        {
            characterActionBar.UpdateActionBarsPercent(playerEntityOnBattle.PercentageActionBar);
            characterActionBar.UpdateActionBarsActions(playerEntityOnBattle.ActionsStack);
            characterActionBar.ShowActionBars(true);
            return;
        }
      
        characterActionBars.Add(playerEntityOnBattle, new CharacterActionBar(playerEntityOnBattle,actionBarPrefab,mainActionBarLayout));
    }

    public void ShowBar(bool value)
    {
        mainActionBarLayout.gameObject.SetActive(value);
    }
}
