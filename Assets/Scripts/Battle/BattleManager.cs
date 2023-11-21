using System;
using System.Collections.Generic;
using Scriptable_Objects.Unit;
using Unity.VisualScripting;
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
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private List<MonsterSO> soMonster = new();
    [SerializeField] private List<Transform> monsterPos = new();
    [SerializeField] private List<Transform> heroPos = new();
    [SerializeField] private Camera exploreCamera;
    [SerializeField] private Camera combatCamera;

    private int nbMonster = 0;
    private Dictionary<string, MonsterSO> Monsters = new();
    private List<Entity> monstersSpawned = new();
    private ActionBattle actionIndex = ActionBattle.Attack;
    private List<PlayerController> playersOnBattle = new();
    private List<PlayerController> playersOnExplore = new();

    // ReSharper disable Unity.PerformanceAnalysis
    public void StartBattle()
    {
        UIBattle.SetActive(true);
        exploreCamera.gameObject.SetActive(false);
        combatCamera.gameObject.SetActive(true);
        nbMonster = Random.Range(1, 4);
        nbMonster = 3;
        for (int i = 0; i < nbMonster; i++)
        {
            Monsters.TryGetValue(soMonster[Random.Range(0, soMonster.Count)].name, out var monster);
            if (monster != null)
            {
                Entity newMonster = Instantiate(monsterPrefab, Vector3.zero, Quaternion.identity).GetComponent<Entity>();
                newMonster.transform.position = monsterPos[i].position;
                newMonster.Init(monster);
                monstersSpawned.Add(newMonster);
            }
        }
        // recup les heros et les spawns
        
    }

    private void Start()
    {
        InputManager.OnSelect += SelectAction;
        InputManager.OnSelection += SelectionAction;
        foreach (var so in soMonster)
            Monsters.Add(so.name, so);
    }

    private void SelectionAction(Direction dir)
    {
        if (dir == Direction.Up)
            actionIndex = (actionIndex == ActionBattle.Attack) ? ActionBattle.Escape : actionIndex - 1;
        else if (dir == Direction.Down)
            actionIndex = (actionIndex == ActionBattle.Escape) ? ActionBattle.Attack : actionIndex + 1;
        OnSelectionChanged?.Invoke((int)actionIndex);
    }

    private void CheckVictory()
    {
        if (monstersSpawned.Count == 0)
        {
            Debug.Log("Victory");
            UIBattle.SetActive(false);
            exploreCamera.gameObject.SetActive(true);
            combatCamera.gameObject.SetActive(false);
            for (int i = 0; i < playersOnBattle.Count; i++)
            {
                playersOnBattle[i].gameObject.SetActive(false);
                playersOnExplore[i].gameObject.SetActive(true);
            }
            GameManager.Instance.GetBackToExplore();
        }
    }

    private void SelectAction()
    {
        switch (actionIndex)
        {
            case ActionBattle.Attack:
                Debug.Log("Attack");
                int indexAttack = 0;
                if (GameManager.Instance.GetPlayerAtIndex(0).Attack(monstersSpawned[indexAttack]))
                {
                    monstersSpawned.RemoveAt(indexAttack);
                    CheckVictory();
                }
                
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

    public void UpdatePlayer(PlayerController player)
    {
        for (int i = 0; i < 1; i++)
        {
            
        }
        if (playersOnBattle.Contains(player))
        {
            //on update les data du player
        }
        else
        {
            playersOnBattle.Add(Instantiate(player.gameObject, heroPos[playersOnBattle.Count].position, Quaternion.identity).GetComponent<PlayerController>());
            playersOnExplore.Add(player);
            player.gameObject.SetActive(false);
        }
    }
}