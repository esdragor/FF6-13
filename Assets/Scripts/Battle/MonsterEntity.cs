using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects.Unit;
using UnityEngine;

public class MonsterEntity : Entity
{
    public static event Action<MonsterEntity> OnAttacking;

    [SerializeField] private float costAttack = 100;
    [SerializeField] private float ratioSpeedActionBar = 1;

    private float actionBar = 0;
    private float speedActionBar = 0;
    private bool currentlyAttacking = false;

    public void ResetValue()
    {
        actionBar = 0;
        speedActionBar = ratioSpeedActionBar * unitData.Agility;
        currentlyAttacking = false;
    }

    IEnumerator AttackAnim(PlayerEntityOnBattle target)
    {
        _spriteRenderer.color = Color.black;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.black;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = selectTarget ? selectTargetColor : Color.white;
        
        target.TakeDamage(unitData.Damage, Elements.None, unitData);
        actionBar = 0;
        currentlyAttacking = false;
    }

    public void Attack(PlayerEntityOnBattle target)
    {
        StartCoroutine(AttackAnim(target));
    }

    private void Update()
    {
        if (actionBar >= costAttack && !currentlyAttacking)
        {
            OnAttacking?.Invoke(this);
            currentlyAttacking = true;
        }
        else
        {
            actionBar += speedActionBar * Time.deltaTime;
        }
    }
}