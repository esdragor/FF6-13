using System;
using Scriptable_Objects.Unit;
using UnityEngine;

namespace Scriptable_Objects.Spells___Effects
{
    [CreateAssetMenu(fileName = "new ElementsResistance", menuName = "ScriptableObjects/Effects/ElementsResistanceSO", order = 0)]
    [Serializable] public class ElementsResistanceSO : EffectSO
    {
        [field:SerializeField] public Elements Element { get; private set; }
        [field:SerializeField] public int Amount { get; private set; }
        [field:SerializeField] public bool FlatValue { get; private set; }
    }
}