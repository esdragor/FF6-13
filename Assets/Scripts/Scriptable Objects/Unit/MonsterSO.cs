using System;
using System.Collections.Generic;
using Scriptable_Objects.Items;
using UnityEngine;

namespace Scriptable_Objects.Unit
{
    [CreateAssetMenu(fileName = "new MonsterSO", menuName = "ScriptableObjects/Units/MonsterSO", order = 0)]
    public class MonsterSO : UnitSO
    {
        [field:Header("---PlayerCharactersSO---")]
        [field:SerializeField] public int Level { get; private set; }
        [field:SerializeField] public int MaxHp { get; private set; }
        [field:SerializeField] public int MaxMp { get; private set; }
        [field:Space]
        [field:SerializeField] public int XpReward { get; private set; }
        [field:SerializeField] public int GilReward { get; private set; }
        [field:Space]
        [field:SerializeField] public int HitRate { get; private set; }
        [field:Space]
        [field:SerializeField] public List<ItemWithChance> StealItems { get; private set; }
        [field:SerializeField] public List<ItemWithChance> DropItems { get; private set; }
        [field:SerializeField] public List<ItemWithChance> MorphItems { get; private set; }
        [field:Space]
        [field:SerializeField] public float StealFailChance { get; private set; }
        [field:SerializeField] public float MorphFailChance { get; private set; }
    }

    [Serializable] public class ItemWithChance
    {
        [field:SerializeField] public ItemSO Item { get; private set; }
        [field:SerializeField] public float Chance { get; private set; }
    }
}