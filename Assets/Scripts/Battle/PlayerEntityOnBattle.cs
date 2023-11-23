using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Scriptable_Objects.Items;
using Scriptable_Objects.Spells___Effects;
using Scriptable_Objects.Unit;
using Units;
using UnityEngine;
using UnityEngine.Serialization;

public class ActionToStack
{
    public ActionBattle action;
    public int index;
    public int Cost;
    public string Name;

    public ActionToStack(ActionBattle action, int cost, string name, int index = 0)
    {
        this.action = action;
        this.index = index;
        Cost = cost;
        Name = name;
    }
}

public class PlayerEntityOnBattle : PlayerEntity
{
    public static event Action<float> OnActionBarChanged;
    public event Action<float> OnActionBarValueChanged;
    public event Action<List<ActionToStack>> OnActonQueueUpdated;

    private List<ActionToStack> actionsStack = new();
    public IReadOnlyList<ActionToStack> ActionsStack => actionsStack;
    private float costOfActionQueue = 0;

    [SerializeField] private float costAttack = 1;
    [field: SerializeField] public float NbBarre { get; private set; } = 2;
    [SerializeField] private float ratioBarre = 50;
    [SerializeField] private float ratioSpeedActionBar = 1;

    private float actionBar;
    private float speedActionBar = 1;
    private bool currentlyAttacking = false;
    private List<Entity> target;
    private bool isSelected = false;


    public bool Attack(int index)
    {
        Debug.Log("Attack");
        if (target == null)
            return false;
        return target[index].TakeDamage(unitData.Damage, Elements.Physical,
            unitData); //Physical for now, need to check the weapon's element
    }

    public bool UseItem(UsableItemSo item, List<Entity> targets)
    {
        Debug.Log($"Use {item.Name} on {targets}");
        // TODO: Should remove the item from the inventory

        var effects = item.Effects.ToList();

        // Items can't miss, so we can do all effects
        foreach (var effect in effects)
        {
            // Check if the effects possess a ignore immunity

            foreach (var tgt in targets)
            {
                switch (effect)
                {
                    case AddOrRemoveAlterationSO addOrRemoveAlterationSo:
                        var ignoreAlteration = effects.Where(e => e is IgnoreAlterationSO).Any(e =>
                            ((IgnoreAlterationSO)e).Alteration == addOrRemoveAlterationSo.Alteration &&
                            ((IgnoreAlterationSO)e).IsPositive);

                        tgt.ApplyAlteration(addOrRemoveAlterationSo.Alteration, ignoreAlteration,
                            addOrRemoveAlterationSo.Remove);
                        break;
                    case DamageEffectSO damageEffectSo:
                        var damage = damageEffectSo.Damage;
                        var element = damageEffectSo.Element;
                        var ignoreDefence = effects.Where(e => e is IgnoreBlockEvadeSO)
                            .Any(e => ((IgnoreBlockEvadeSO)e).IgnoreBlock);
                        var isPourcentDamage = !damageEffectSo.FlatValue;
                        tgt.TakeDamage(damage, element, unitData, ignoreDefence, isPourcentDamage);
                        break;
                    case MpRegenSO mpRegenSo:
                        tgt.RegenMpDamage(mpRegenSo.Regen, !mpRegenSo.FlatValue);
                        break;
                }
            }
        }

        return true;
    }

    public bool UseSpell(SpellSO spell, List<Entity> targets)
    {
        Debug.Log($"Use {spell.Name} on targets");

        var effects = spell.SpellEffects.ToList();
        var onehit = false;
        var hit = false;

        // Remove Mp ; Ap

        foreach (var tgt in targets)
        {
            //TODO: Check if the spell hit
            //spell.HitRate
            hit = true; // It seams player can't miss attacks (but enemies can dodge)

            if (spell.HitRate == 0) hit = true;
            if (!hit) continue;

            foreach (var effect in effects)
            {
                switch (effect)
                {
                    case AddOrRemoveAlterationSO addOrRemoveAlterationSo:
                        var ignoreAlteration = effects.Where(e => e is IgnoreAlterationSO).Any(e =>
                            ((IgnoreAlterationSO)e).Alteration == addOrRemoveAlterationSo.Alteration &&
                            ((IgnoreAlterationSO)e).IsPositive);

                        tgt.ApplyAlteration(addOrRemoveAlterationSo.Alteration, ignoreAlteration,
                            addOrRemoveAlterationSo.Remove);
                        break;
                    case DamageEffectSO damageEffectSo:
                        var damage = damageEffectSo.Damage;
                        if (damageEffectSo.ScaleWithPower) damage *= spell.Power;
                        var element = damageEffectSo.Element;
                        var ignoreDefence = effects.Where(e => e is IgnoreBlockEvadeSO)
                            .Any(e => ((IgnoreBlockEvadeSO)e).IgnoreBlock);
                        var isPourcentDamage = !damageEffectSo.FlatValue;
                        tgt.TakeDamage(damage, element, unitData, ignoreDefence, isPourcentDamage);
                        break;
                    case MpRegenSO mpRegenSo:
                        tgt.RegenMpDamage(mpRegenSo.Regen, !mpRegenSo.FlatValue);
                        break;
                }
            }
        }

        return hit;
    }

    public void InitForBattle()
    {
        ResetValues();

        ShowSelector(false);
    }

    public void ExitBattle()
    {
        ResetValues();

        ShowSelector(false);
    }

    public void ResetValues()
    {
        actionBar = 0;
        costOfActionQueue = 0;
        speedActionBar = ratioSpeedActionBar * unitData.Agility;
        currentlyAttacking = false;
        isSelected = false;
    }

    public void AddActionToQueue(ActionBattle action, int index = 0)
    {
        int nbAction = 0;
        switch (action)
        {
            case ActionBattle.AutoAttack:
                nbAction = (int)((NbBarre * ratioBarre) / (costAttack * ratioBarre - costOfActionQueue));
                for (int i = 0; i < nbAction; i++)
                {
                    actionsStack.Add(new ActionToStack(action, (int)costAttack, "Attack"));
                    costOfActionQueue += costAttack * ratioBarre;
                }
                break;
            case ActionBattle.Attack:
                if ((NbBarre * ratioBarre) < (costAttack * ratioBarre + costOfActionQueue)) break;
                    actionsStack.Add(new ActionToStack(action, (int)costAttack, "Attack")); 
                    costOfActionQueue += costAttack * ratioBarre;
                break;
            case ActionBattle.Abilities:
                var ability = ((PlayerCharacterData)unitData).getAllSpells()[index];
                int cost = ability.ApCost;
                costOfActionQueue += cost * ratioBarre;
                actionsStack.Add(new ActionToStack(action, cost, ability.Name, index));
                break;
            case ActionBattle.Items:
                Debug.Log("Items");
                break;
        }

        OnActonQueueUpdated?.Invoke(actionsStack);
    }

    public void SpendActionBar(float value)
    {
        actionBar -= value;
        if (actionBar < 0)
            actionBar = 0;
        OnActionBarValueChanged?.Invoke(PercentageActionBar);
        if (isSelected) OnActionBarChanged?.Invoke(PercentageActionBar);
    }

    public float PercentageActionBar => actionBar / (NbBarre * ratioBarre);

    IEnumerator DequeueAction()
    {
        currentlyAttacking = true;
        bool success = false;
            ActionBattle action = actionsStack[0].action;
            int index = actionsStack[0].index;
            actionsStack.RemoveAt(0);
            OnActonQueueUpdated?.Invoke(actionsStack);
            switch (action)
            {
                case ActionBattle.AutoAttack or ActionBattle.Attack:
                    Vector3 targetPos = Vector3.zero;
                    Vector3 originalPos = transform.position;
                    if (target[index] != null)
                    {
                        targetPos = target[index].transform.position;
                        
                    }
                    if (target.Count > 0)
                    {
                        transform.DOMove(targetPos + Vector3.right, 1f);
                        yield return new WaitForSeconds(1f);

                        success = Attack(index);
                        costOfActionQueue -= costAttack * ratioBarre;
                        if (success)
                            SpendActionBar(costAttack * ratioBarre);

                        if (success) yield return new WaitForSeconds(0.3f); //animation attack
                        transform.DOMove(originalPos, 1f);
                        yield return new WaitForSeconds(1f);
                    }
                    else
                    {
                        Debug.Log("No target");
                        costOfActionQueue -= costAttack * ratioBarre;
                        yield return new WaitForSeconds(1f);
                    }

                    break;
                case ActionBattle.Abilities:
                    UseSpell((unitData as PlayerCharacterData)?.getAllSpells()[index], target);
                    break;


                case ActionBattle.Items:
                    Debug.Log("Item");
                    break;
            }
        OnActonQueueUpdated?.Invoke(actionsStack);

        currentlyAttacking = false;
    }

    private void Update()
    {
        if (actionBar >= (NbBarre * ratioBarre))
        {
            actionBar = (NbBarre * ratioBarre);
            //Debug.Log("actionBar is full");
        }
        else
        {
            actionBar += speedActionBar * Time.deltaTime;
            OnActionBarValueChanged?.Invoke(PercentageActionBar);
            if (isSelected) OnActionBarChanged?.Invoke(PercentageActionBar);
        }

        //Debug.Log("D: " + actionBar);
        if (costOfActionQueue > 0 && actionBar >= costOfActionQueue && !currentlyAttacking)
        {
            Debug.Log("Dequeue");
            Debug.Log("Cost: " + costOfActionQueue);
            Debug.Log("Action: " + actionBar);
            StartCoroutine(DequeueAction());
        }
    }

    public void addTarget(Entity entity)
    {
        if (target == null)
            target = new List<Entity>();
        else
            target.Clear();
        target.Add(entity);
    }

    public void SelectPlayer()
    {
        isSelected = true;
    }

    public static void TrySelectPlayer(PlayerEntityOnBattle previous, PlayerEntityOnBattle current)
    {
        if (previous != null) previous.ShowSelector(false);
        if (current != null) current.ShowSelector(true);
    }
}