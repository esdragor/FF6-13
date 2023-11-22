using System;
using System.Collections;
using System.Collections.Generic;
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

public class BattleManager : MonoBehaviour
{
    public static event Action<List<PlayerEntityOnBattle>> OnPlayerUpdated;
    public static event Action OnBattleStarted;
    public static event Action OnBattleEnded;
    public static event Action<int> OnSelectionChanged;
    public static event Action<PlayerEntityOnBattle,PlayerEntityOnBattle> OnCharacterSelected; 

    [SerializeField] private GameObject UIBattle;
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
    int indexSpells = 0;
    bool selectTarget = false;
    bool openSpellList = false;
    List<SpellSO> spells = new();

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

    public void StartBattle(PlayerController playerController)
    {
        UIBattle.SetActive(true);
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
        InputManager.OnSelect += SelectAction;
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
        
        OnCharacterSelected?.Invoke(previous,playersOnBattle[indexPlayer]);
    }

    IEnumerator Victory() // changer par l'ecran de victoire
    {
        yield return new WaitForSeconds(2f);
        OnBattleEnded?.Invoke();
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

    public void SelectionEnemyTarget(Direction dir)
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

        string name = "";
        
        if (!openSpellList)
            name = monstersSpawned[indexTarget].name;
        else
            if (indexTarget < monstersSpawned.Count)
                name = monstersSpawned[indexTarget].name;
            else
                name = playersOnBattle[indexTarget - monstersSpawned.Count].name;
        Debug.Log("Target : " + name);
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

        if (openSpellList)
        {
            SelectionSpell(dir);
            return;
        }


        if (dir == Direction.Up)
            actionIndex = (actionIndex == ActionBattle.AutoAttack) ? ActionBattle.Items : actionIndex - 1;
        else if (dir == Direction.Down)
            actionIndex = (actionIndex == ActionBattle.Items) ? ActionBattle.AutoAttack : actionIndex + 1;
        OnSelectionChanged?.Invoke((int)actionIndex);
    }

    private void SelectionSpell(Direction dir)
    {
        if (dir == Direction.Down)
            indexSpells = (indexSpells == 0) ? spells.Count - 1 : indexSpells - 1;
        else if (dir == Direction.Up)
            indexSpells = (indexSpells == spells.Count - 1) ? 0 : indexSpells + 1;
        Debug.Log("Spell : " + spells[indexSpells].name);
    }

    private void SelectAction()
    {
        switch (actionIndex)
        {
            case ActionBattle.AutoAttack:
                Debug.Log("Attack");
                if (selectTarget)
                {
                    PlayerEntityOnBattle player = GetPlayerAtIndex(indexPlayer);
                    monstersSpawned[indexTarget].DeselectTarget();
                    player.addTarget(monstersSpawned[indexTarget]);
                    player.AddActionToQueue(ActionBattle.AutoAttack);
                    selectTarget = false;
                }
                else
                {
                    selectTarget = true;
                    indexTarget = 0;
                    monstersSpawned[indexTarget].SelectTarget();
                }

                break;
            case ActionBattle.Abilities:
                if (openSpellList == false)
                {
                    spells.Clear();
                    spells = (playersOnBattle[indexPlayer].unitData as PlayerCharacterData)?.getAllSpells();
                    Debug.Log(spells.Count + " spells found");
                    if (spells != null)
                    {
                        // j'open la liste des spells
                        openSpellList = true;
                        indexSpells = 0;
                    }
                }
                else if (openSpellList && !selectTarget)
                {
                    selectTarget = true;
                    // je highlight la target
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
                    if (indexTarget < monstersSpawned.Count)
                    {
                        monstersSpawned[indexTarget].DeselectTarget();
                        player.addTarget(monstersSpawned[indexTarget]);
                    }
                    else
                    {
                        GetPlayerAtIndex(indexTarget - monstersSpawned.Count).DeselectTarget();
                        player.addTarget(GetPlayerAtIndex(indexTarget - monstersSpawned.Count));
                    }
                    player.AddActionToQueue(ActionBattle.Abilities, indexSpells);
                    selectTarget = false;
                    openSpellList = false;
                }

                Debug.Log("Abilities");
                break;
            case ActionBattle.Items:
                Debug.Log("Items");
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
                playersOnBattle[i].InitForBattle();
                playersOnBattle[i].transform.position = heroPos[i].position;
            }
            
            //playersOnBattle[indexPlayer].SelectPlayer();

            //on update les data du player
            //PlayerControllerOnBattle controller = playersOnBattle[playersOnBattle.IndexOf(player)];
            //player.getEntity().InitData(player.getEntity().unitData);
            //controller.getEntity().gameObject.SetActive(true);
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
        
        OnPlayerUpdated?.Invoke(playersOnBattle);
        
        ChangeCharacter(0);
    }
}