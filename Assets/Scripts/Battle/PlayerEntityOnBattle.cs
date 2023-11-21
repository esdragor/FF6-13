using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntityOnBattle : PlayerEntity
{
    public static event Action<float> OnActionBarChanged;
    
    [SerializeField] private float limitActionBar = 100;
    [SerializeField] private float speedActionBar = 1;
    
    float actionBar = 0;
    
    public bool Attack(Entity target)
    {
        Debug.Log("Attack");
        return target.TakeDamage(unitData.Attack, unitData);
    }
    
    public void ResetActionBar()
    {
        actionBar = 0;
    }
    
    public bool SpendActionBar(float value)
    {
        if (actionBar >= value)
        {
            actionBar -= value;
            OnActionBarChanged?.Invoke(GetPercentageActionBar());
            return true;
        }
        return false;
    }
    
    public float GetPercentageActionBar()
    {
        return (actionBar / limitActionBar) * 0.01f;
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
            OnActionBarChanged?.Invoke(GetPercentageActionBar());
        }
    }
}
