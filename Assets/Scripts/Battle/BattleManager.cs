using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects.Encounters;
using Scriptable_Objects.Items;
using Scriptable_Objects.Unit;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum ActionBattle
{
    AutoAttack,
    Attack,
    Abilities,
    Items,
    Summon,
    Defend,
    Row
}

public class BattleManager : MonoBehaviour
{
    public static event Action<List<PlayerEntityOnBattle>, InterBattle> OnPlayerUpdated;
    public static event Action OnBattleStarted;
    public static event Action OnBattleEnded;
    public static event Action OnBattleFinished;
    public static event Action<string> OnActionWasLaunched;
    public static event Action<int> OnSelectionChanged;
    public static event Action<PlayerEntityOnBattle, PlayerEntityOnBattle> OnCharacterSelected;
    public static event Action<string> OnGainXP;

    [SerializeField] private MonsterEntity monsterPrefab;
    [SerializeField] private List<MonsterSO> soMonster = new();
    [SerializeField] private List<Transform> monsterPos = new();
    [SerializeField] private List<Transform> heroPos = new();
    [SerializeField] private Camera exploreCamera;
    [SerializeField] private Camera combatCamera;
    [SerializeField] private SpriteRenderer backgroundRenderer;
    [SerializeField] private PlayerControllerOnBattle playerBattlePrefab;

    private int nbMonster = 0;
    private Dictionary<string, MonsterSO> Monsters = new();
    public List<Entity> monstersSpawned { get; private set; } = new();
    private ActionBattle actionIndex = ActionBattle.AutoAttack;
    private PlayerControllerOnBattle playerControllerBattle;
    public List<PlayerEntityOnBattle> playersOnBattle { get; private set; } = new();
    private PlayerController playersOnExplore;

    static int gilsReward = 0;
    static int xpReward = 0;
    private bool multiTarget = false;
    private bool endBattle = false;
    private string LevelUp = "";
    private ActionBattle currentAction = ActionBattle.AutoAttack;
    private bool debugMode = true;
    private BattleLoop battleLoop;

    private void Awake()
    {
        Entity.OnEntityDying += RemoveEntity;
    }

    private void Start()
    {
        battleLoop = new BattleLoop(this);
        InputManager.OnSelect += SelectionPressed;
        InputManager.OnChangeCharacter += ChangeCharacter;
        MonsterEntity.OnAttacking += RetrieveTarget;
        foreach (var so in soMonster)
            Monsters.Add(so.name, so);
    }

    private void RemoveEntity(Entity entity)
    {
        if (monstersSpawned.Contains(entity))
        {
            Destroy(entity.gameObject, 5f);
            monstersSpawned.Remove(entity);
            CheckVictory();
        }
        else
        {
            if (CheckDefeat()) return;
            if (entity is PlayerEntityOnBattle player)
            {
                if (playersOnBattle[battleLoop.indexPlayer] == player)
                {
                    ChangeCharacter(1);
                }
            }
        }
    }

    private void SelectionPressed()
    {
        if (!endBattle) return;
        GetBackToWorld();
    }

    public static void ActionLaunched(string name)
    {
        OnActionWasLaunched?.Invoke(name);
    }

    public void StartBattle(PlayerController playerController, EncounterSO encounterSo)
    {
        //UIBattle.SetActive(true);
        exploreCamera.gameObject.SetActive(false);
        combatCamera.gameObject.SetActive(true);
        nbMonster = encounterSo.MonstersReadOnly.Count;
        gilsReward = 0;
        xpReward = 0;
        endBattle = false;
        battleLoop.Reset();
        for (int i = 0; i < nbMonster; i++)
        {
            MonsterEntity newMonster = Instantiate(monsterPrefab, Vector3.zero, Quaternion.identity);
            newMonster.transform.position = monsterPos[i].position;
            newMonster.Init(encounterSo.MonstersReadOnly[i].Monster, true);
            newMonster.ResetValue();
            monstersSpawned.Add(newMonster);
        }

        backgroundRenderer.sprite = encounterSo.Background;

        OnBattleStarted?.Invoke();

        UpdatePlayer(playerController);
    }

    public void StartBattle(PlayerController playerController)
    {
        //UIBattle.SetActive(true);
        exploreCamera.gameObject.SetActive(false);
        combatCamera.gameObject.SetActive(true);
        nbMonster = Random.Range(1, 4);
        if (debugMode)
            nbMonster = 3;
        gilsReward = 0;
        xpReward = 0;
        endBattle = false;
        battleLoop.Reset();
        for (int i = 0; i < nbMonster; i++)
        {
            Monsters.TryGetValue(soMonster[Random.Range(0, soMonster.Count)].name, out var monster);
            if (monster != null)
            {
                MonsterEntity newMonster = Instantiate(monsterPrefab, Vector3.zero, Quaternion.identity);
                newMonster.transform.position = monsterPos[i].position;
                newMonster.Init(monster, true);
                newMonster.ResetValue();
                monstersSpawned.Add(newMonster);
            }
        }

        OnBattleStarted?.Invoke();

        UpdatePlayer(playerController);
    }

    public void RetrieveTarget(MonsterEntity monster)
    {
        List<PlayerEntityOnBattle> potentialTarget = new List<PlayerEntityOnBattle>();
        foreach (var player in playersOnBattle)
        {
            if (player.unitData.CurrentHp > 0)
                potentialTarget.Add(player);
        }

        if (potentialTarget.Count == 0) return;
        int index = Random.Range(0, potentialTarget.Count);
        monster.Attack(potentialTarget[index]);
    }

    private void ChangeCharacter(float axis)
    {
        var previous = playersOnBattle[battleLoop.indexPlayer];

        do
        {
            battleLoop.indexPlayer += (int)axis;
            if (battleLoop.indexPlayer < 0)
                battleLoop.indexPlayer = playersOnBattle.Count - 1;
            else if (battleLoop.indexPlayer >= playersOnBattle.Count)
                battleLoop.indexPlayer = 0;
        } while (playersOnBattle[battleLoop.indexPlayer].unitData.CurrentHp <= 0 &&
                 previous != playersOnBattle[battleLoop.indexPlayer]);

        OnCharacterSelected?.Invoke(previous, playersOnBattle[battleLoop.indexPlayer]);
    }

    private void GetBackToWorld()
    {
        OnBattleFinished?.Invoke();
        for (int i = 0; i < playersOnBattle.Count; i++)
        {
            playersOnBattle[i].gameObject.SetActive(false);
        }

        playerControllerBattle.gameObject.SetActive(false);
        exploreCamera.gameObject.SetActive(true);
        combatCamera.gameObject.SetActive(false);
        playersOnExplore.gameObject.SetActive(true);
        playersOnExplore.getEntity().gameObject.SetActive(true);
        GameManager.Instance.GetBackToExplore();
        //HideScore();
    }

    IEnumerator Victory()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Victory");
        endBattle = true;
        OnBattleEnded?.Invoke();

        for (int i = 0; i < playersOnBattle.Count; i++)
        {
            playersOnBattle[i].ClearAllActions();
        }

        playersOnExplore.AddGils(gilsReward);
        LevelUp = playersOnExplore.AddXP(xpReward);
        OnGainXP?.Invoke(LevelUp);
        playersOnExplore.UpdateInventory(playersOnBattle[0].Inventory);
        playersOnExplore.UpdateTeam();
    }

    private void CheckVictory()
    {
        if (monstersSpawned.Count == 0)
        {
            StartCoroutine(Victory());
        }
    }

    private bool CheckDefeat()
    {
        bool defeat = true;
        foreach (var player in playersOnBattle)
        {
            if (player.unitData.CurrentHp > 0)
            {
                defeat = false;
                break;
            }
        }

        if (defeat)
        {
            SceneManager.LoadScene(0);
        }

        return defeat;
    }

    public static void AddXPToLoot(int xp)
    {
        xpReward += xp;
    }

    public static void AddGilsToLoot(int gils)
    {
        //add gils to the player
        gilsReward += gils;
    }

    public static void AddGilsAndXpToLoot(int gils, int xp)
    {
        AddGilsToLoot(gils);
        AddXPToLoot(xp);
    }

    public static void AddItemToLoot(ItemSO item, int quantity)
    {
        //add item to the player
    }

    public static void GetLoot(out int gils, out int xp, out List<ItemSO> items)
    {
        gils = gilsReward;
        xp = xpReward;
        items = new List<ItemSO>();
    }


    public static void AbortItemSelection()
    {
        if (BattleLoop.openItemList)
        {
            BattleLoop.openItemList = false;
            BattleLoop.selectTarget = false;
        }
    }

    public PlayerEntityOnBattle GetPlayerAtIndex(int index)
    {
        return playersOnBattle[index];
    }

    private void UpdatePlayer(PlayerController player)
    {
        if (playersOnBattle.Count > 0)
        {
            playerControllerBattle.gameObject.SetActive(true);
            for (int i = 0; i < playersOnBattle.Count; i++)
            {
                //controller.getEntity().UpdateData(player.getEntity().unitData);
                playersOnBattle[i].InitData((i == 0) ? player.getEntity().unitData : player.companions[i - 1].unitData);
                playersOnBattle[i].gameObject.SetActive(true);
                playersOnBattle[i].InitForBattle(player.inventoryItems);
                playersOnBattle[i].transform.position = heroPos[i].position;
            }

            if (playersOnBattle[battleLoop.indexPlayer].unitData.CurrentHp <= 0) ChangeCharacter(1);
        }
        else
        {
            playerControllerBattle = Instantiate(playerBattlePrefab, heroPos[0].position, Quaternion.identity);
            playersOnExplore = player;
            playerControllerBattle.InitPlayer();

            PlayerEntityOnBattle principalPlayer = playerControllerBattle.getEntity();
            principalPlayer.Init(player.getEntity().SO);
            principalPlayer.InitData(player.getEntity().unitData);
            principalPlayer.InitForBattle(player.inventoryItems);

            playersOnBattle.Add(principalPlayer);

            //add here creation des autres persos
            for (int i = 0; i < player.companions.Count; i++)
            {
                PlayerEntityOnBattle companion = Instantiate(principalPlayer);
                companion.Init(player.companions[i].SO);
                companion.InitData(player.companions[i].unitData);
                companion.InitForBattle(player.inventoryItems);
                companion.transform.position = heroPos[i + 1].position;
                playersOnBattle.Add(companion);
            }

            playersOnBattle[battleLoop.indexPlayer].SelectPlayer();
            player.gameObject.SetActive(false);
            player.getEntity().gameObject.SetActive(false);
        }

        OnPlayerUpdated?.Invoke(playersOnBattle, battleLoop);

        ChangeCharacter(0);
        playersOnBattle[0].OnTakeDamage();
    }
}