using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects.Items;
using UnityEngine;

public class CombatUIDisplayer : MonoBehaviour
{
    [Header("General")] [SerializeField] private UIRefs controlsControls;
    [SerializeField] private UIRefs nextControls;

    [Header("Main Panels")] [SerializeField]
    private UIPanel teamPanelObj;

    [SerializeField] private GameObject enemyPanelObj;

    [Header("Selection Panels")] [SerializeField]
    private CombatActionSelectionDisplayer combatActionSelectionDisplayer;

    [Header("Selection Info")] [SerializeField]
    private UIPanel selectionInfoPanel;

    [SerializeField] private GameObject selectionInfoPanelDisabledObj;

    [Header("Main Action Bar")] [SerializeField]
    private CombatActionBarDisplayer mainActionBarDisplayer;

    [Header("Ability Name")] [SerializeField]
    private UIPanel abilityNamePanel;

    [Header("Battle End Panels")] [SerializeField]
    private UIPanel endBattleTopPanel; //probably rewards

    [SerializeField] private UIPanel endBattleXPPanel;

    private IReadOnlyList<PlayerEntityOnBattle> playerEntitiesOnBattle;
    

    private void OnEnable()
    {
        BattleManager.OnBattleStarted += Show;
        BattleManager.OnPlayerUpdated += SetPlayerEntities;
        PlayerEntity.OnPlayerLifeUpdated += UpdateCharactersInfo;
        BattleManager.OnCharacterSelected += PlayerEntityOnBattle.TrySelectPlayer;
        BattleManager.OnCharacterSelected += mainActionBarDisplayer.ShowActionBar;
        BattleManager.OnCharacterSelected += combatActionSelectionDisplayer.ShowActionSelector;
        BattleManager.OnActionWasLaunched += ShowAbilityName;
        BattleManager.OnBattleEnded += Hide;
        BattleManager.OnBattleEnded += ShowEndPanel;
        BattleManager.OnBattleFinished += Hide;
        BattleManager.OnGainXP += UpdateLevelUp;
    }

    private void OnDisable()
    {
        BattleManager.OnBattleStarted -= Show;
        BattleManager.OnPlayerUpdated -= SetPlayerEntities;
        BattleManager.OnCharacterSelected -= PlayerEntityOnBattle.TrySelectPlayer;
        BattleManager.OnCharacterSelected -= combatActionSelectionDisplayer.ShowActionSelector;
        BattleManager.OnBattleEnded -= Hide;
        BattleManager.OnActionWasLaunched -= ShowAbilityName;
        BattleManager.OnBattleEnded -= ShowEndPanel;
        BattleManager.OnBattleFinished -= Hide;
        BattleManager.OnGainXP -= UpdateLevelUp;
        PlayerEntity.OnPlayerLifeUpdated -= UpdateCharactersInfo;

    }

    private void Start()
    {
        Hide();
    }

    private void Hide()
    {
        teamPanelObj.gameObject.SetActive(false);
        enemyPanelObj.SetActive(false);
        selectionInfoPanel.gameObject.SetActive(false);
        selectionInfoPanelDisabledObj.SetActive(true);
        endBattleTopPanel.gameObject.SetActive(false);
        endBattleXPPanel.gameObject.SetActive(false);

        HideEndPanel();
        HideAbilityName();
        mainActionBarDisplayer.Hide();
        combatActionSelectionDisplayer.Hide();
    }

    public void UpdateCharactersInfo()
    {
        string text = "";
        foreach (var entity in playerEntitiesOnBattle)
        {
            text += entity.PlayerCharactersSo.Name
                    + "  "
                    + entity.unitData.CurrentHp
                    + "/"
                    + entity.unitData.MaxHp
                    + "\n";
        }
        teamPanelObj.SetText(text);
    }

    public void Show()
    {
        HideEndPanel();
        teamPanelObj.gameObject.SetActive(true);
        enemyPanelObj.SetActive(true);
    }

    public void SetPlayerEntities(IReadOnlyList<PlayerEntityOnBattle> playerEntities, InterBattle manager)
    {
        playerEntitiesOnBattle = playerEntities;
        mainActionBarDisplayer.CreateActionBars(playerEntitiesOnBattle);
        combatActionSelectionDisplayer.CreateSelectors(playerEntitiesOnBattle, manager);
    }

    public void ShowEndPanel()
    {
        endBattleTopPanel.gameObject.SetActive(true);
        endBattleXPPanel.gameObject.SetActive(true);
        BattleManager.GetLoot(out var gils, out var xp, out List<ItemSO> items);
        endBattleTopPanel.SetText("Gil " + gils + "\nEXP " + xp);
    }
    
    private void UpdateLevelUp(string lvlUp)
    {
        endBattleXPPanel.SetText(lvlUp);
    }

    public void HideEndPanel()
    {
        endBattleTopPanel.gameObject.SetActive(false);
        endBattleXPPanel.gameObject.SetActive(false);
    }

    private IEnumerator HideAbilityNameAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        HideAbilityName();
    }

    public void ShowAbilityName(string abilityName)
    {
        abilityNamePanel.SetText(abilityName);
        abilityNamePanel.gameObject.SetActive(true);
        StartCoroutine(HideAbilityNameAfterSeconds(1f));
    }

    public void HideAbilityName()
    {
        abilityNamePanel.gameObject.SetActive(false);
    }
}