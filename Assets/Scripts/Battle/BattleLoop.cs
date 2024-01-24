using System;
using System.Collections.Generic;
using Scriptable_Objects.Spells___Effects;
using Units;
using UnityEngine;

public class BattleLoop : InterBattle
{
    public static event Action<PlayerEntityOnBattle> OnShowSpellList;
    public static event Action<PlayerEntityOnBattle> OnShowItemList;
    public static event Action OnStartSelectionUI;
    public static event Action OnEndSelectionUI;
    public static event Action<ActionBattle> OnSelectAction;
    
    public static int indexSpells = 0;
    public static int indexItems = 0;
    public static bool openItemList = false;
    public static bool selectTarget = false;
    static bool openSpellList = false;
    public int indexPlayer {get; set;} = 0;
    public int indexTarget {get; private set;} = 0;
    
    private List<SpellSO> spells = new();
    private BattleManager manager;
    
    public BattleLoop(BattleManager _manager)
    {
        InputManager.OnSelection += SelectionAction;
        manager = _manager;
        OnSelectAction += ActionSelected;
    }
    
    public static void NeedToSelectSpellTarget(bool need, int _indexSpell)
    {
        if (need)
        {
            OnSelectAction?.Invoke(ActionBattle.Abilities);
            OnEndSelectionUI?.Invoke();
        }
        else
            OnStartSelectionUI?.Invoke();

        selectTarget = need;
        indexSpells = _indexSpell;
    }
    
    public static void NeedToSelectItemTarget(bool need, int _indexItem)
    {
        if (need)
        {
            OnSelectAction?.Invoke(ActionBattle.Items);
            OnEndSelectionUI?.Invoke();
        }
        else
            OnStartSelectionUI?.Invoke();

        selectTarget = need;
        indexItems = _indexItem;
    }
    
    public void ActionSelected(ActionBattle action)
    {
        switch (action)
        {
            case ActionBattle.AutoAttack:
                break;
            case ActionBattle.Attack:
                break;
            case ActionBattle.Abilities:
                LaunchAbilities();
                break;
            case ActionBattle.Items:
                LaunchItems();
                break;
            case ActionBattle.Summon:
                break;
            case ActionBattle.Defend:
                break;
            case ActionBattle.Row:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(action), action, null);
        }
    }

    public static void AbortSpellSelection()
    {
        if (openSpellList)
        {
            openSpellList = false;
            selectTarget = false;
        }
    }
    
    public void Reset()
    {
        indexPlayer = 0;
        selectTarget = false;
        openSpellList = false;
    }
    
    private void LaunchItems()
    {
        Debug.Log("Items");
        if (!openItemList)
        {
            // items.Clear();
            Inventory items = manager.playersOnBattle[indexPlayer].Inventory;
            if (items != null)
            {
                // j'open la liste des Items
                openItemList = true;
                indexItems = 0;
                OnShowItemList?.Invoke(manager.playersOnBattle[indexPlayer]);
            }
        }
        else if (openItemList && !selectTarget)
        {
            selectTarget = true;
            indexTarget = 0;
            manager.monstersSpawned[indexTarget].SelectTarget();
        }
        else
        {
            // je lance l'item
            PlayerEntityOnBattle player = manager.GetPlayerAtIndex(indexPlayer);
            if (indexTarget < manager.monstersSpawned.Count) manager.monstersSpawned[indexTarget].DeselectTarget();
            else manager.GetPlayerAtIndex(indexTarget - manager.monstersSpawned.Count).DeselectTarget();
            List<Entity> target = new List<Entity>();
            if (indexTarget < manager.monstersSpawned.Count)
            {
                manager.monstersSpawned[indexTarget].DeselectTarget();
                target.Add(manager.monstersSpawned[indexTarget]);
            }
            else
            {
                manager.GetPlayerAtIndex(indexTarget - manager.monstersSpawned.Count).DeselectTarget();
                target.Add(manager.GetPlayerAtIndex(indexTarget - manager.monstersSpawned.Count));
            }

            player.AddActionToQueue(ActionBattle.Items, target, indexItems);
            selectTarget = false;
            openItemList = false;
            NeedToSelectItemTarget(false, -1);
        }
    }
    
    private void LaunchAbilities()
    {
        Debug.Log("Abilities");
        if (!openSpellList)
        {
            spells.Clear();
            spells = (manager.playersOnBattle[indexPlayer].unitData as PlayerCharacterData)?.getAllSpells();
            Debug.Log(spells.Count + " spells found");
            if (spells != null)
            {
                // j'open la liste des spells
                openSpellList = true;
                indexSpells = 0;
                OnShowSpellList?.Invoke(manager.playersOnBattle[indexPlayer]);
            }
        }
        else if (openSpellList && !selectTarget)
        {
            selectTarget = true;
            indexTarget = 0;
            indexTarget = (spells[0].SpellType == SpellTypes.Heal) ? indexTarget = manager.monstersSpawned.Count : 0;
            if (indexTarget < manager.monstersSpawned.Count) manager.monstersSpawned[indexTarget].SelectTarget();
            else manager.GetPlayerAtIndex(indexTarget - manager.monstersSpawned.Count).SelectTarget();
        }
        else
        {
            // je lance le spell
            PlayerEntityOnBattle player = manager.GetPlayerAtIndex(indexPlayer);
            if (indexTarget < manager.monstersSpawned.Count) manager.monstersSpawned[indexTarget].DeselectTarget();
            else manager.GetPlayerAtIndex(indexTarget - manager.monstersSpawned.Count).DeselectTarget();
            List<Entity> target = new List<Entity>();
            if (indexTarget < manager.monstersSpawned.Count)
            {
                manager.monstersSpawned[indexTarget].DeselectTarget();
                target.Add(manager.monstersSpawned[indexTarget]);
            }
            else
            {
                manager.GetPlayerAtIndex(indexTarget - manager.monstersSpawned.Count).DeselectTarget();
                target.Add(manager.GetPlayerAtIndex(indexTarget - manager.monstersSpawned.Count));
            }

            player.AddActionToQueue(ActionBattle.Abilities, target, indexSpells);
            selectTarget = false;
            openSpellList = false;
            NeedToSelectSpellTarget(false, -1);
        }
    }
    
    
    public Action GetAction(ActionBattle actionBattle)
    {
        return actionBattle switch
        {
            ActionBattle.AutoAttack => () => LaunchAttack(false), //la methode de baudouin la
            ActionBattle.Attack => () => LaunchAttack(true), // la methode de baudouin la mais non
            ActionBattle.Abilities => LaunchAbilities, // ability panel
            ActionBattle.Items => LaunchItems, // items panel
            ActionBattle.Summon => () => Debug.Log("Summon"), // lol non
            ActionBattle.Defend => () => Debug.Log("Defend"), // jsp
            ActionBattle.Row => () => Debug.Log("Row"), // jsp
            _ => () => Debug.Log("Wat")
        };
    }
    
    private void SelectionAction(Direction dir)
    {
        if (selectTarget) SelectionEnemyTarget(dir);
    }
    
    private void SelectionEnemyTarget(Direction dir)
    {
        int oldIndex = indexTarget;

        indexTarget = dir switch
        {
            Direction.Down => (indexTarget == 0) ? manager.playersOnBattle.Count + manager.monstersSpawned.Count - 1 : indexTarget - 1,
            Direction.Up => (indexTarget == manager.playersOnBattle.Count + manager.monstersSpawned.Count - 1) ? 0 : indexTarget + 1,
            _ => indexTarget
        };
        if (oldIndex == indexTarget) return;
        if (indexTarget < manager.monstersSpawned.Count) manager.monstersSpawned[indexTarget].SelectTarget();
        else manager.GetPlayerAtIndex(indexTarget - manager.monstersSpawned.Count).SelectTarget();
        if (oldIndex < manager.monstersSpawned.Count) manager.monstersSpawned[oldIndex].DeselectTarget();
        else manager.GetPlayerAtIndex(oldIndex - manager.monstersSpawned.Count).DeselectTarget();
    }
    
    private void LaunchAttack(bool single)
    {
        if (!selectTarget)
        {
            selectTarget = true;
            indexTarget = 0;
            if (indexTarget >= manager.monstersSpawned.Count) return;
            manager.monstersSpawned[indexTarget].SelectTarget();
            OnEndSelectionUI?.Invoke();
        }
        else
        {
            PlayerEntityOnBattle MyPlayer = manager.GetPlayerAtIndex(indexPlayer);
            if (indexTarget >= manager.monstersSpawned.Count) return;
            manager.monstersSpawned[indexTarget].DeselectTarget();
            List<Entity> targets = new List<Entity>();
            targets.Add(manager.monstersSpawned[indexTarget]);
            MyPlayer.AddActionToQueue(single ? ActionBattle.Attack : ActionBattle.AutoAttack, targets);
            selectTarget = false;
            OnStartSelectionUI?.Invoke();
        }
    }
}
