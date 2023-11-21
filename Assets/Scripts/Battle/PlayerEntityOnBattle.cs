using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Scriptable_Objects.Items;
using Scriptable_Objects.Spells___Effects;
using Units;
using UnityEngine;

public class PlayerEntityOnBattle : PlayerEntity
{
    public static event Action<float> OnActionBarChanged;

    private List<ActionBattle> actionsQueue = new();
    private float costOfActionQueue = 0;

    public float costAttack { get; private set; }
    [SerializeField] private float limitActionBar = 100;
    [SerializeField] private float ratioSpeedActionBar = 1;

    private float actionBar = 0;
    private float speedActionBar = 1;
    private bool currentlyAttacking = false;
    private Entity target;

    public bool Attack()
    {
        Debug.Log("Attack");
        if (target == null)
            return false;
        return target.TakeDamage(unitData.Attack, unitData);
    }
    
    public bool UseItem(UsableItemSo item, List<UnitData> targets)
    {
        Debug.Log($"Use {item.Name} on {targets}");
        var effects = item.Effects.ToList();
        
        // Items can't miss, so we can do all effects
        foreach (var effect in effects)
        {
            
            // Check if the effects possess a ignore immunity
            
            
            
            foreach (var tgt in targets) {
                switch (effect)
                {
                    case AddOrRemoveAlterationSO addOrRemoveAlterationSo:
                        var ignoreAlteration = effects.Where(e => e is IgnoreAlterationSO).Any(e => ((IgnoreAlterationSO)e).Alteration == addOrRemoveAlterationSo.Alteration && ((IgnoreAlterationSO)e).IsPositive);
                        
                        if (addOrRemoveAlterationSo.Remove)
                        {
                            tgt.RemoveAlteration(addOrRemoveAlterationSo.Alteration, ignoreAlteration);
                            break;
                        }
                        tgt.AddAlteration(addOrRemoveAlterationSo.Alteration, ignoreAlteration);
                        break;
                }
            }
        }
        
        return false;
    }

    public void ResetValues()
    {
        actionBar = 0;
        costAttack = 50f;
        costOfActionQueue = 0;
        speedActionBar = ratioSpeedActionBar * unitData.Agility;
        currentlyAttacking = false;
    }

    public void addActionToQueue(ActionBattle action, int index = 0)
    {
        int nbAction = 0;
        switch (action)
        {
            case ActionBattle.AutoAttack:
                nbAction = (int)(limitActionBar / costAttack);
                for (int i = 0; i < nbAction; i++)
                {
                    actionsQueue.Add(action);
                    costOfActionQueue += costAttack;
                }

                break;
            case ActionBattle.Abilities:
                Debug.Log("Defend");
                break;
            case ActionBattle.Items:
                Debug.Log("Item");
                break;
        }
    }

    public void SpendActionBar(float value)
    {
        actionBar -= value;
        if (actionBar < 0)
            actionBar = 0;
        OnActionBarChanged?.Invoke(GetPercentageActionBar());
    }

    public float GetPercentageActionBar()
    {
        return (actionBar / limitActionBar);
    }

    IEnumerator DequeueAllAction()
    {
        currentlyAttacking = true;
        bool success = false;
        while (actionsQueue.Count > 0)
        {
            ActionBattle action = actionsQueue[0];
            actionsQueue.RemoveAt(0);
            switch (action)
            {
                case ActionBattle.AutoAttack:
                    Vector3 originalPos = transform.position;
                    if (target)
                    {
                        transform.DOMove(target.transform.position + Vector3.right, 1f);
                        yield return new WaitForSeconds(1f);


                        success = Attack();

                        if (success) yield return new WaitForSeconds(1f); //animation attack
                        transform.DOMove(originalPos, 1f);
                        yield return new WaitForSeconds(1f);
                        costOfActionQueue -= costAttack;
                        if (!success) break;
                        SpendActionBar(costAttack);
                    }
                    else
                    {
                        Debug.Log("No target");
                        costOfActionQueue -= costAttack;
                        yield return new WaitForSeconds(1f);
                    }
                    break;
                case ActionBattle.Abilities:
                    Debug.Log("Abilities");
                    break;
                case ActionBattle.Items:
                    Debug.Log("Item");
                    break;
            }
        }

        currentlyAttacking = false;
    }

    private void Update()
    {
        if (actionBar >= limitActionBar)
        {
            actionBar = limitActionBar;
        }
        else
        {
            actionBar += speedActionBar * Time.deltaTime;
            //Debug.Log(actionBar);
            OnActionBarChanged?.Invoke(GetPercentageActionBar());
        }

        if (actionBar >= costOfActionQueue && !currentlyAttacking)
            StartCoroutine(DequeueAllAction());
    }

    public void addTarget(Entity entity)
    {
        target = entity;
    }
}