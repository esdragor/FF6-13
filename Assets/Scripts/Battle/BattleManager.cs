using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects.Items;
using Scriptable_Objects.Spells___Effects;
using Scriptable_Objects.Unit;
using Units;
using UnityEngine;
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

public class BattleManager : MonoBehaviour, InterBattle
{
    public static event Action<List<PlayerEntityOnBattle>, BattleManager> OnPlayerUpdated;
    public static event Action OnBattleStarted;
    public static event Action OnBattleEnded;
    public static event Action<int> OnSelectionChanged;
    public static event Action<PlayerEntityOnBattle, PlayerEntityOnBattle> OnCharacterSelected;
    public static event Action<ActionBattle> OnSelectAction;

    public static event Action OnStartSelectionUI;
    public static event Action OnEndSelectionUI;
    public static event Action<PlayerEntityOnBattle> OnShowSpellList;
    public static event Action<PlayerEntityOnBattle> OnShowItemList;

    //[SerializeField] private GameObject UIBattle;
    [SerializeField] private MonsterEntity monsterPrefab;
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
    int indexTarget = 0;
    static int indexSpells = 0;
    static int indexItems = 0;
    static bool selectTarget = false;
    bool openSpellList = false;
    bool openItemList = false;
    List<SpellSO> spells = new();
    List<UsableItemSo> items = new();
    ActionBattle currentAction = ActionBattle.AutoAttack;

    private void Awake()
    {
        Entity.OnEntityDying += RemoveEntity;
        OnSelectAction += ActionSelected;
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
    
    private void ActionSelected(ActionBattle action)
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

    public void StartBattle(PlayerController playerController, List<MonsterSO> monsters)
    {
        //UIBattle.SetActive(true);
        exploreCamera.gameObject.SetActive(false);
        combatCamera.gameObject.SetActive(true);
        nbMonster = monsters.Count;
        indexPlayer = 0;
        openSpellList = false;
        selectTarget = false;
        for (int i = 0; i < nbMonster; i++)
        {
            MonsterEntity newMonster = Instantiate(monsterPrefab, Vector3.zero, Quaternion.identity);
            newMonster.transform.position = monsterPos[i].position;
            newMonster.Init(monsters[i], true);
            newMonster.ResetValue();
            monstersSpawned.Add(newMonster);
        }

        OnBattleStarted?.Invoke();

        UpdatePlayer(playerController);
    }

    public void StartBattle(PlayerController playerController)
    {
        //UIBattle.SetActive(true);
        exploreCamera.gameObject.SetActive(false);
        combatCamera.gameObject.SetActive(true);
        nbMonster = Random.Range(1, 4);
        nbMonster = 3;
        indexPlayer = 0;
        openSpellList = false;
        selectTarget = false;
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

    private void Start()
    {
        //InputManager.OnSelect += SelectAction;
        InputManager.OnSelection += SelectionAction;
        InputManager.OnChangeCharacter += ChangeCharacter;
        MonsterEntity.OnAttacking += RetrieveTarget;
        foreach (var so in soMonster)
            Monsters.Add(so.name, so);
    }

    public void RetrieveTarget(MonsterEntity monster)
    {
        int index = Random.Range(0, playersOnBattle.Count);
        monster.Attack(playersOnBattle[index]);
    }

    private void ChangeCharacter(float axis)
    {
        var previous = playersOnBattle[indexPlayer];

        indexPlayer += (int)axis;
        if (indexPlayer < 0)
            indexPlayer = playersOnBattle.Count - 1;
        else if (indexPlayer >= playersOnBattle.Count)
            indexPlayer = 0;

        OnCharacterSelected?.Invoke(previous, playersOnBattle[indexPlayer]);
    }

    IEnumerator Victory() // changer par l'ecran de victoire
    {
        yield return new WaitForSeconds(2f);
        OnBattleEnded?.Invoke();
        //UIBattle.SetActive(false);
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

    private void LaunchAttack(bool single)
    {
        if (!selectTarget)
        {
            //currentAction = ActionBattle.AutoAttack;
            selectTarget = true;
            indexTarget = 0;
            monstersSpawned[indexTarget].SelectTarget();
            OnEndSelectionUI?.Invoke();
        }
        else
        {
            //currentAction = ActionBattle.AutoAttack;
            PlayerEntityOnBattle MyPlayer = GetPlayerAtIndex(indexPlayer);
            monstersSpawned[indexTarget].DeselectTarget();
            List<Entity> targets = new List<Entity>();
            targets.Add(monstersSpawned[indexTarget]);
            MyPlayer.AddActionToQueue(single ? ActionBattle.Attack : ActionBattle.AutoAttack, targets);
            selectTarget = false;
            OnStartSelectionUI?.Invoke();
        }
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

    private void LaunchItems()
    {
         Debug.Log("Items");
        if (!openItemList)
        {
            // items.Clear();
            items = (playersOnBattle[indexPlayer].unitData as PlayerCharacterData)?.getAllItems();
            Debug.Log(items.Count + " items found");
            if (items != null)
            {
                // j'open la liste des Items
                openItemList = true;
                indexItems = 0;
                OnShowItemList?.Invoke(playersOnBattle[indexPlayer]);
            }
        }
        else if (openItemList && !selectTarget)
        {
            selectTarget = true;
            indexTarget = 0;
           monstersSpawned[indexTarget].SelectTarget();
        }
        else
        {
           
            // je lance l'item
            PlayerEntityOnBattle player = GetPlayerAtIndex(indexPlayer);
            if (indexTarget < monstersSpawned.Count) monstersSpawned[indexTarget].DeselectTarget();
            else GetPlayerAtIndex(indexTarget - monstersSpawned.Count).DeselectTarget();
            List<Entity> target = new List<Entity>();
            if (indexTarget < monstersSpawned.Count)
            {
                monstersSpawned[indexTarget].DeselectTarget();
                target.Add(monstersSpawned[indexTarget]);
            }
            else
            {
                GetPlayerAtIndex(indexTarget - monstersSpawned.Count).DeselectTarget();
                target.Add(GetPlayerAtIndex(indexTarget - monstersSpawned.Count));
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
            spells = (playersOnBattle[indexPlayer].unitData as PlayerCharacterData)?.getAllSpells();
            Debug.Log(spells.Count + " spells found");
            if (spells != null)
            {
                // j'open la liste des spells
                openSpellList = true;
                indexSpells = 0;
                OnShowSpellList?.Invoke(playersOnBattle[indexPlayer]);
            }
        }
        else if (openSpellList && !selectTarget)
        {
            
            selectTarget = true;
            indexTarget = 0;
            indexTarget = (spells[0].SpellType == SpellTypes.Heal) ? indexTarget = monstersSpawned.Count : 0;
            if (indexTarget < monstersSpawned.Count) monstersSpawned[indexTarget].SelectTarget();
            else GetPlayerAtIndex(indexTarget - monstersSpawned.Count).SelectTarget();
        }
        else
        {
            // je lance le spell
            PlayerEntityOnBattle player = GetPlayerAtIndex(indexPlayer);
            if (indexTarget < monstersSpawned.Count) monstersSpawned[indexTarget].DeselectTarget();
            else GetPlayerAtIndex(indexTarget - monstersSpawned.Count).DeselectTarget();
            List<Entity> target = new List<Entity>();
            if (indexTarget < monstersSpawned.Count)
            {
                monstersSpawned[indexTarget].DeselectTarget();
                target.Add(monstersSpawned[indexTarget]);
            }
            else
            {
                GetPlayerAtIndex(indexTarget - monstersSpawned.Count).DeselectTarget();
                target.Add(GetPlayerAtIndex(indexTarget - monstersSpawned.Count));
            }
            player.AddActionToQueue(ActionBattle.Abilities, target, indexSpells);
            selectTarget = false;
            openSpellList = false;
            NeedToSelectSpellTarget(false, -1);
        }
    }

    private void SelectionEnemyTarget(Direction dir)
    {
        int oldIndex = indexTarget;
        if (openSpellList)
        {
            //parcours les enemis et les players
        }

        if (dir == Direction.Down)
        {
            if (!openSpellList)
                indexTarget = (indexTarget == 0) ? monstersSpawned.Count - 1 : indexTarget - 1;
            if (openSpellList)
                indexTarget = (indexTarget == 0) ? playersOnBattle.Count + monstersSpawned.Count - 1 : indexTarget - 1;
        }
        else if (dir == Direction.Up)
        {
            if (!openSpellList)
                indexTarget = (indexTarget == monstersSpawned.Count - 1) ? 0 : indexTarget + 1;
            if (openSpellList)
                indexTarget = (indexTarget == playersOnBattle.Count + monstersSpawned.Count - 1) ? 0 : indexTarget + 1;
        }
        
        if (!openSpellList && oldIndex != indexTarget)
        {
            monstersSpawned[oldIndex].DeselectTarget();
            monstersSpawned[indexTarget].SelectTarget();
        }
        else if (openSpellList && oldIndex != indexTarget)
        {
            if (indexTarget < monstersSpawned.Count) monstersSpawned[indexTarget].SelectTarget();
            else GetPlayerAtIndex(indexTarget - monstersSpawned.Count).SelectTarget();
            if (oldIndex < monstersSpawned.Count) monstersSpawned[oldIndex].DeselectTarget();
            else GetPlayerAtIndex(oldIndex - monstersSpawned.Count).DeselectTarget();
        }
    }

    private void SelectionAction(Direction dir)
    {
        if (selectTarget)
        {
            SelectionEnemyTarget(dir);
            return;
        }
    }
    private PlayerEntityOnBattle GetPlayerAtIndex(int index)
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
                playersOnBattle[i].InitData(player.getEntity().unitData);
                playersOnBattle[i].gameObject.SetActive(true);
                playersOnBattle[i].InitForBattle();
                playersOnBattle[i].transform.position = heroPos[i].position;
            }
        }
        else
        {
            playerControllerBattle = Instantiate(playerBattlePrefab, heroPos[0].position, Quaternion.identity);
            playersOnExplore = player;
            playerControllerBattle.InitPlayer();

            PlayerEntityOnBattle principalPlayer = playerControllerBattle.getEntity();
            principalPlayer.Init(player.getEntity().SO);
            principalPlayer.InitData(player.getEntity().unitData);
            principalPlayer.InitForBattle();

            playersOnBattle.Add(principalPlayer);

            //add here creation des autres persos
            for (int i = 0; i < player.companions.Count; i++)
            {
                PlayerEntityOnBattle companion = Instantiate(principalPlayer);
                companion.Init(player.companions[i].SO);
                companion.InitData(player.companions[i].unitData);
                companion.InitForBattle();
                companion.transform.position = heroPos[i + 1].position;
                playersOnBattle.Add(companion);
            }

            playersOnBattle[indexPlayer].SelectPlayer();
            player.gameObject.SetActive(false);
            player.getEntity().gameObject.SetActive(false);
        }

        OnPlayerUpdated?.Invoke(playersOnBattle, this);

        ChangeCharacter(0);
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
}