using System;
using System.Collections;
using System.Collections.Generic;
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

                    success = Attack();
                    costOfActionQueue -= costAttack;
                    if (!success) break;
                    SpendActionBar(costAttack);
                    break;
                case ActionBattle.Abilities:
                    Debug.Log("Defend");
                    break;
                case ActionBattle.Items:
                    Debug.Log("Item");
                    break;
            }

            if (success)
            {
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
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
            Debug.Log(actionBar);
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