using System.Collections.Generic;
using Scriptable_Objects.Spells___Effects;
using Scriptable_Objects.Unit;
using UnityEngine;

namespace Scriptable_Objects.Items
{
    [CreateAssetMenu(fileName = "New Armor", menuName = "ScriptableObjects/Items/ArmorSO", order = 0)]
    public class ArmorSO : EquipableSO
    {
        [field: Header("---ArmorSO---")] 
        [field: SerializeField] public int Defense { get; private set; }
        [field: SerializeField] public int MagicDefense { get; private set; }
        [field: SerializeField] public int Evasion { get; private set; }
        [field: SerializeField] public int MagicEvasion { get; private set; }
        [field: SerializeField] public List<ElementResistance> Resistances { get; private set; }
    }
}