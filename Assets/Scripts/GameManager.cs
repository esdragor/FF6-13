using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects.Unit;
using UI_Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum State
{
    Battle,
    Explore,
    Dialog,
    None
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private BattleManager _battleManager;
    [SerializeField] private UIManager _uiManager;
    
    private State _state;
    private PlayerEntity _player;
    
    private bool DebugBattle = false;
    
    private IEnumerator TryLaunchBattle()
    {
        yield return new WaitForSeconds(0.1f);
        int random = Random.Range(0, 100);
        if ((random < 20 && _state != State.Dialog && _state != State.Battle) || DebugBattle)
        {
            LaunchBattleRandom();
        }
    }

    private void LaunchBattleRandom()
    {
        Debug.Log("Battle");
        _state = State.Battle;
        _inputManager.OnBattle();
        _battleManager.StartBattle(_player._playerController);
    }
    
    public void LaunchBattleScripted(List<MonsterSO> monsters)
    {
        Debug.Log("Battle");
        _state = State.Battle;
        _inputManager.OnBattle();
        _battleManager.StartBattle(_player._playerController, monsters);
    }

    public void GetBackToExplore()
    {
        _state = State.Dialog;
        _inputManager.OnExploration();
        //StartCoroutine(LaunchBattle());
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        PlayerController.OnPlayerSpawned += SetTarget;
    }

    private void SetTarget(Entity _entity)
    {
        _player = _entity as PlayerEntity;
    }

    void Start()
    {
        GetBackToExplore();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchBattleRandom();
            
            // _state = State.Battle;
            // _inputManager.OnBattle();
            // _battleManager.UpdatePlayer(_player._playerController);
            // _battleManager.StartBattle();
        }
    }
}