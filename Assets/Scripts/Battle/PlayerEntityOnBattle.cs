using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntityOnBattle : PlayerEntity
{
    public bool Attack(Entity target)
    {
        Debug.Log("Attack");
        return target.TakeDamage(unitData.Attack, unitData);
    }
}
