using System.Collections.Generic;
using Scriptable_Objects.Unit;
using UnityEngine;

namespace Scriptable_Objects.Items
{
    [CreateAssetMenu(fileName = "New Armor", menuName = "ScriptableObjects/Items/ArmorSO", order = 0)]
    public class ArmorSO : ScriptableObject
    {
        [field: Header("---ArmorSO---")] 
        [field: SerializeField] public List<ElementResistance> Resistances { get; private set; }
    }
}