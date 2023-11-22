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

public class ActionToQueue
{
    public ActionBattle action;
    public int index;

    public ActionToQueue(ActionBattle action, int index = 0)
    {
        this.action = action;
        this.index = index;
    }
}

public class PlayerEntityOnBattle : PlayerEntity
{
    public static event Action<float> OnActionBarChanged;

    private List<ActionToQueue> actionsQueue = new();
    private float costOfActionQueue = 0;

    [SerializeField] private float costAttack = 1;
    [SerializeField] private float nbBarre = 2;
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
        return target[index].TakeDamage(unitData.Attack, Elements.Physical,
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
                        var damage = damageEffectSo.Damage ;
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

    public void ResetValues()
    {
        actionBar = 0;
        costOfActionQueue = 0;
        speedActionBar = ratioSpeedActionBar * unitData.Agility;
        currentlyAttacking = false;
        isSelected = false;
    }

    public void addActionToQueue(ActionBattle action, int index = 0)
    {
        int nbAction = 0;
        switch (action)
        {
            case ActionBattle.AutoAttack:
                nbAction = (int)((nbBarre * ratioBarre) / (costAttack * ratioBarre - costOfActionQueue));
                for (int i = 0; i < nbAction; i++)
                {
                    actionsQueue.Add(new ActionToQueue(action));
                    costOfActionQueue += costAttack * ratioBarre;
                }

                break;
            case ActionBattle.Abilities:
                int cost = (unitData as PlayerCharacterData).getAllSpells()[index].ApCost;
                costOfActionQueue += cost * ratioBarre;
                actionsQueue.Add(new ActionToQueue(action, index));
                break;
            case ActionBattle.Items:
                Debug.Log("Items");
                break;
        }
    }

    public void SpendActionBar(float value)
    {
        actionBar -= value;
        if (actionBar < 0)
            actionBar = 0;
        if (isSelected)
            OnActionBarChanged?.Invoke(GetPercentageActionBar());
    }

    public float GetPercentageActionBar()
    {
        return (actionBar / (nbBarre * ratioBarre));
    }

    IEnumerator DequeueAllAction()
    {
        currentlyAttacking = true;
        bool success = false;
        while (actionsQueue.Count > 0)
        {
            ActionBattle action = actionsQueue[0].action;
            int index = actionsQueue[0].index;
            actionsQueue.RemoveAt(0);
            switch (action)
            {
                case ActionBattle.AutoAttack:
                    Vector3 originalPos = transform.position;
                    if (target.Count > 0)
                    {
                        transform.DOMove(target[index].transform.position + Vector3.right, 1f);
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

                    UseSpell((unitData as PlayerCharacterData).getAllSpells()[index], target);
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
        if (actionBar >= (nbBarre * ratioBarre))
        {
            actionBar = (nbBarre * ratioBarre);
            //Debug.Log("actionBar is full");
        }
        else
        {
            actionBar += speedActionBar * Time.deltaTime;
            if (isSelected)
                OnActionBarChanged?.Invoke(GetPercentageActionBar());
        }

        //Debug.Log("D: " + actionBar);
        if (costOfActionQueue > 0 && actionBar >= costOfActionQueue && !currentlyAttacking)
            StartCoroutine(DequeueAllAction());
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
}