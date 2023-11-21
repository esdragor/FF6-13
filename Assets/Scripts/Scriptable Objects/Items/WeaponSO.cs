using System.Collections.Generic;
using Scriptable_Objects.Unit;
using UnityEngine;

namespace Scriptable_Objects.Items
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/Items/WeaponSO", order = 0)]
    public class WeaponSO : EquipableSO
    {
        [field:Header("---WeaponSO---")]
        [field:SerializeField] public int AttackPower { get; private set; }
        [field:SerializeField] public int HitRate { get; private set; }
        [field:SerializeField] public Elements Element { get; private set; }
        [field:SerializeField] public bool OneHanded { get; private set; }
        [field:SerializeField] public bool Melee { get; private set; }
        [field:SerializeField] public List<PlayerCharactersSO> UsableBy { get; private set; }
    }
}