using System;
using System.Collections;
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
    private PlayerControllerOnBattle playerControllerBattle;
    private List<PlayerEntityOnBattle> playersOnBattle = new();
    private PlayerController playersOnExplore;

    int indexPlayer = 0;

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
        indexPlayer = 0;
        for (int i = 0; i < nbMonster; i++)
        {
            Monsters.TryGetValue(soMonster[Random.Range(0, soMonster.Count)].name, out var monster);
            if (monster != null)
            {
                Entity newMonster = Instantiate(monsterPrefab, Vector3.zero, Quaternion.identity)
                    .GetComponent<Entity>();
                newMonster.transform.position = monsterPos[i].position;
                newMonster.Init(monster, true);
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
        indexPlayer += (int)axis;
        if (indexPlayer < 0)
            indexPlayer = playersOnBattle.Count - 1;
        else if (indexPlayer >= playersOnBattle.Count)
            indexPlayer = 0;
    }

    private void SelectionAction(Direction dir)
    {
        if (dir == Direction.Up)
            actionIndex = (actionIndex == ActionBattle.AutoAttack) ? ActionBattle.Items : actionIndex - 1;
        else if (dir == Direction.Down)
            actionIndex = (actionIndex == ActionBattle.Items) ? ActionBattle.AutoAttack : actionIndex + 1;
        OnSelectionChanged?.Invoke((int)actionIndex);
    }

    IEnumerator Victory() // changer par l'ecran de victoire
    {
        yield return new WaitForSeconds(2f);
        UIBattle.SetActive(false);
        exploreCamera.gameObject.SetActive(true);
        combatCamera.gameObject.SetActive(false);
        playersOnExplore.gameObject.SetActive(true);
        playersOnExplore.getEntity().gameObject.SetActive(true);
        for (int i = 0; i < playersOnBattle.Count; i++)
        {
            playerControllerBattle.gameObject.SetActive(false);
            playersOnBattle[i].gameObject.SetActive(false);
        }

        GameManager.Instance.GetBackToExplore();
    }

    private void CheckVictory()
    {
        if (monstersSpawned.Count == 0)
        {
            Debug.Log("Victory");
            StartCoroutine(Victory());

        }
    }

    private void SelectAction()
    {
        switch (actionIndex)
        {
            case ActionBattle.AutoAttack:
                Debug.Log("Attack");
                PlayerEntityOnBattle player = GetPlayerAtIndex(indexPlayer);
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
        if (playersOnBattle.Count > 0)
        {
            playerControllerBattle.gameObject.SetActive(true);
            for (int i = 0; i < playersOnBattle.Count; i++)
            {
                //controller.getEntity().UpdateData(player.getEntity().unitData);
                playersOnBattle[i].InitData(player.getEntity().unitData);
                playersOnBattle[i].gameObject.SetActive(true);
                playersOnBattle[i].ResetValues();
                playersOnBattle[i].transform.position = heroPos[i].position;
            }
            playersOnBattle[indexPlayer].SelectPlayer();

            //on update les data du player
            //PlayerControllerOnBattle controller = playersOnBattle[playersOnBattle.IndexOf(player)];
            //player.getEntity().InitData(player.getEntity().unitData);
            //controller.getEntity().gameObject.SetActive(true);
        }
        else
        {
            PlayerControllerOnBattle controller =
                Instantiate(playerBattlePrefab.gameObject, heroPos[0].position, Quaternion.identity)
                    .GetComponent<PlayerControllerOnBattle>();
            playerControllerBattle = controller;
            playersOnExplore = player;
            controller.InitPlayer();

            PlayerEntityOnBattle principalPlayer = controller.getEntity();
            principalPlayer.Init(player.getEntity().SO);
            principalPlayer.InitData(player.getEntity().unitData);
            principalPlayer.ResetValues();

            playersOnBattle.Add(principalPlayer);

            //add here creation des autres persos
            for (int i = 0; i < player.companions.Count; i++)
            {
                PlayerEntityOnBattle companion =
                    Instantiate(principalPlayer.gameObject).GetComponent<PlayerEntityOnBattle>();
                companion.Init(player.companions[i].SO);
                companion.InitData(player.companions[i].unitData);
                companion.ResetValues();
                companion.transform.position = heroPos[i + 1].position;
                playersOnBattle.Add(companion);
            }
            playersOnBattle[indexPlayer].SelectPlayer();
            player.gameObject.SetActive(false);
            player.getEntity().gameObject.SetActive(false);
        }
    }
}