using System;
using System.Collections.Generic;
using Scriptable_Objects.Unit;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ActionBattle
{
    AutoAttack,
    Abilities,
    Items
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
    [SerializeField] private PlayerControllerOnBattle playerBattlePrefab;

    private int nbMonster = 0;
    private Dictionary<string, MonsterSO> Monsters = new();
    private List<Entity> monstersSpawned = new();
    private ActionBattle actionIndex = ActionBattle.AutoAttack;
    private PlayerControllerOnBattle playerControllerBattle = new();
    private List<PlayerEntityOnBattle> playersOnBattle = new();
    private List<PlayerController> playersOnExplore = new();

    private void Awake()
    {
        Entity.OnEntityDying += RemoveEntity;
    }

    private void RemoveEntity(Entity entity)
    {
        if (monstersSpawned.Contains(entity))
        {
            monstersSpawned.Remove(entity);
            CheckVictory();
        }
        else
        {
            //one player is dead
        }
    }

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
                Entity newMonster = Instantiate(monsterPrefab, Vector3.zero, Quaternion.identity)
                    .GetComponent<Entity>();
                newMonster.transform.position = monsterPos[i].position;
                newMonster.Init(monster);
                monstersSpawned.Add(newMonster);
            }
        }
    }

    private void Start()
    {
        InputManager.OnSelect += SelectAction;
        InputManager.OnSelection += SelectionAction;
        InputManager.OnChangeCharacter += ChangeCharacter;
        foreach (var so in soMonster)
            Monsters.Add(so.name, so);
    }

    private void ChangeCharacter(float axis)
    {
        Debug.Log("Change Character");
        Debug.Log(axis);
    }

    private void SelectionAction(Direction dir)
    {
        if (dir == Direction.Up)
            actionIndex = (actionIndex == ActionBattle.AutoAttack) ? ActionBattle.Items : actionIndex - 1;
        else if (dir == Direction.Down)
            actionIndex = (actionIndex == ActionBattle.Items) ? ActionBattle.AutoAttack : actionIndex + 1;
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
                playerControllerBattle.gameObject.SetActive(false);
                playersOnBattle[i].gameObject.SetActive(false);
                playersOnExplore[i].gameObject.SetActive(true);
                playersOnExplore[i].getEntity().gameObject.SetActive(true);
            }

            GameManager.Instance.GetBackToExplore();
        }
    }

    private void SelectAction()
    {
        switch (actionIndex)
        {
            case ActionBattle.AutoAttack:
                Debug.Log("Attack");
                PlayerEntityOnBattle player = GetPlayerAtIndex(0);
                int indexTarget = Random.Range(0, monstersSpawned.Count);
                player.addTarget(monstersSpawned[indexTarget]);
                player.addActionToQueue(ActionBattle.AutoAttack);

                break;
            case ActionBattle.Abilities:
                Debug.Log("Defend");
                break;
            case ActionBattle.Items:
                Debug.Log("Item");
                break;
        }
    }

    private PlayerEntityOnBattle GetPlayerAtIndex(int index)
    {
        return playersOnBattle[index];
    }

    public void UpdatePlayer(PlayerController player)
    {
        for (int i = 0; i < 1; i++)
        {
        }

        if (playersOnBattle.Count > 0)
        {
            for (int i = 0; i < playersOnBattle.Count; i++)
            {
                //controller.getEntity().UpdateData(player.getEntity().unitData);
                playersOnBattle[i].gameObject.SetActive(true);
            }

            //on update les data du player
            //PlayerControllerOnBattle controller = playersOnBattle[playersOnBattle.IndexOf(player)];
            //controller.getEntity().UpdateData(player.getEntity().unitData);
            //controller.getEntity().gameObject.SetActive(true);
        }
        else
        {
            PlayerControllerOnBattle controller =
                Instantiate(playerBattlePrefab.gameObject, heroPos[0].position, Quaternion.identity)
                    .GetComponent<PlayerControllerOnBattle>();
            playerControllerBattle = controller;
            playersOnExplore.Add(player);
            controller.InitPlayer();
            
            PlayerEntityOnBattle principalPlayer = controller.getEntity();
            principalPlayer.Init(player.getEntity().SO);
            principalPlayer.InitData(player.getEntity().unitData);
            principalPlayer.ResetValues();
            
            playersOnBattle.Add(principalPlayer);
            
            
            //add here creation des autres persos


            player.gameObject.SetActive(false);
            player.getEntity().gameObject.SetActive(false);
        }
    }
}