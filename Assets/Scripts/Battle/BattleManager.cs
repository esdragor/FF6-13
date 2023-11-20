using System;
using System.Collections.Generic;
using Scriptable_Objects.Unit;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ActionBattle
{
    Attack,
    Defend,
    Item,
    Escape
}

public class BattleManager : MonoBehaviour
{
    public static event Action OnBattleStarted;
    public static event Action<int> OnSelectionChanged;
    
    [SerializeField] private GameObject UIBattle;
    [SerializeField] private Dictionary<string, MonsterSO> Monsters = new();
    [SerializeField] private List<string> monstersNames = new();

    private int nbMonster = 0;
    private List<MonsterSO> monstersToSpawn = new();
    private ActionBattle actionIndex = ActionBattle.Attack;

    public void StartBattle()
    {
        UIBattle.SetActive(true);
        nbMonster = Random.Range(1, 4);
        for (int i = 0; i < nbMonster; i++)
        {
            Monsters.TryGetValue(monstersNames[Random.Range(0, monstersNames.Count)], out var monster);
            monstersToSpawn.Add(monster);
        }
    }

    private void Start()
    {
        InputManager.OnSelect += SelectAction;
        InputManager.OnSelection += SelectionAction;
    }

    private void SelectionAction(Direction dir)
    {
        if (dir == Direction.Up)
            actionIndex = (actionIndex == ActionBattle.Attack) ? ActionBattle.Escape : actionIndex - 1;
        else if (dir == Direction.Down)
            actionIndex = (actionIndex == ActionBattle.Escape) ? ActionBattle.Attack : actionIndex + 1;
        OnSelectionChanged?.Invoke((int) actionIndex);
    }


    private void SelectAction()
    {
        switch (actionIndex)
        {
            case ActionBattle.Attack:
                Debug.Log("Attack");
                break;
            case ActionBattle.Defend:
                Debug.Log("Defend");
                break;
            case ActionBattle.Item:
                Debug.Log("Item");
                break;
            case ActionBattle.Escape:
                Debug.Log("Escape");
                break;
        }
    }
}