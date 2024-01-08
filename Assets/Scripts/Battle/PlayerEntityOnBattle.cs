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
    public List<Entity> target;
    public int index;
    public int Cost;
    public string Name;

    public ActionToStack(ActionBattle _action, int cost, string name, List<Entity> t, int _index = 0)
    {
        action = _action;
        index = _index;
        Cost = cost;
        Name = name;
        target = new List<Entity>();
        target.AddRange(t);
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
    private bool isSelected = false;
    private Inventory inventory;


    public Inventory Inventory => inventory;

    public void ClearAllActions()
    {
        actionsStack.Clear();
        costOfActionQueue = 0;
        OnActonQueueUpdated?.Invoke(actionsStack);
    }

    public bool Attack(List<Entity> targets)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null)
                return false;
            targets[i].TakeDamage(unitData.Damage, Elements.Physical,
                unitData); //Physical for now, need to check the weapon's element
        }

        return true;
    }

    public bool UseItem(UsableItemSo item, List<Entity> targets)
    {
        if (inventory == null)
        {
            Debug.Log("Inventory is null");
            return false;
        }

        if (inventory.HasItem(item) == false)
        {
            Debug.Log("Item not in inventory");
            return false;
        }

        inventory.RemoveItem(item, 1);
        Debug.Log($"Use {item.Name} on targets");

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

    public void InitForBattle(Inventory _inventory)
    {
        ResetValues();
        if (_inventory.Items == null) return;

        inventory = new Inventory();
        foreach (var item in _inventory.Items)
        {
            inventory.AddItem(item.Item, item.Quantity);
        }

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

    public void AddActionToQueue(ActionBattle action, List<Entity> target, int index = 0)
    {
        int nbAction = 0;
        switch (action)
        {
            case ActionBattle.AutoAttack:
                nbAction = (costAttack * ratioBarre - costOfActionQueue) == 0
                    ? 1
                    : (int)((NbBarre * ratioBarre) / (costAttack * ratioBarre - costOfActionQueue));
                for (int i = 0; i < nbAction; i++)
                {
                    actionsStack.Add(new ActionToStack(action, (int)costAttack, "Attack", target));
                    costOfActionQueue += costAttack * ratioBarre;
                }

                break;
            case ActionBattle.Attack:
                if ((NbBarre * ratioBarre) < (costAttack * ratioBarre + costOfActionQueue)) break;
                actionsStack.Add(new ActionToStack(action, (int)costAttack, "Attack", target));
                costOfActionQueue += costAttack * ratioBarre;
                break;
            case ActionBattle.Abilities:
                var ability = ((PlayerCharacterData)unitData).getAllSpells()[index];
                int cost = ability.ApCost;
                costOfActionQueue += cost * ratioBarre;
                actionsStack.Add(new ActionToStack(action, cost, ability.Name, target, index));
                break;
            case ActionBattle.Items:
                Debug.Log("Items");
                var item = inventory.Items[index].Item;
                int costItem = 1;
                costOfActionQueue += costItem * ratioBarre;
                actionsStack.Add(new ActionToStack(action, costItem, item.Name, target, index));
                break;
        }

        OnActonQueueUpdated?.Invoke(actionsStack);
    }

    public void SpendActionBar(float value)
    {
        actionBar -= value;
        costOfActionQueue -= value;

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
        List<Entity> target = actionsStack[0].target;
        int cost = actionsStack[0].Cost;
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

                    success = Attack(target);
                    if (success)
                        SpendActionBar(cost * ratioBarre);

                    if (success) yield return new WaitForSeconds(0.3f); //animation attack
                    transform.DOMove(originalPos, 1f);
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    Debug.Log("No target");
                    yield return new WaitForSeconds(1f);
                }

                break;
            case ActionBattle.Abilities:
                SpellSO spell = ((PlayerCharacterData)unitData).getAllSpells()[index];
                if (UseSpell(spell, target))
                {
                    SpendActionBar(cost * ratioBarre);
                    BattleManager.ActionLaunched(spell.Name);
                }

                break;


            case ActionBattle.Items:
                UsableItemSo item = inventory.Items[index].Item;
                if (UseItem(item, target))
                {
                    SpendActionBar(cost * ratioBarre);
                    BattleManager.ActionLaunched(item.Name);
                }

                break;
        }

        OnActonQueueUpdated?.Invoke(actionsStack);

        yield return new WaitForSeconds(1.0f); // animation delay between actions

        currentlyAttacking = false;
    }

    private void Update()
    {
        if (actionBar >= (NbBarre * ratioBarre))
        {
            actionBar = (NbBarre * ratioBarre);
        }
        else
        {
            actionBar += speedActionBar * Time.deltaTime;
            OnActionBarValueChanged?.Invoke(PercentageActionBar);
            if (isSelected) OnActionBarChanged?.Invoke(PercentageActionBar);
        }

        if (costOfActionQueue > 0 && actionBar >= costOfActionQueue && !currentlyAttacking)
        {
            StartCoroutine(DequeueAction());
        }
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