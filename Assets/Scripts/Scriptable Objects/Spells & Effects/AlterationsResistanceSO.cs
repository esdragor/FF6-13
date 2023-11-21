using System;
using Scriptable_Objects.Unit;
using UnityEngine;

namespace Scriptable_Objects.Spells___Effects
{
    [CreateAssetMenu(fileName = "new AlterationsResistance", menuName = "ScriptableObjects/Effects/AlterationsResistanceSO", order = 0)]
    [Serializable] public class AlterationsResistanceSO : EffectSO
    {
        [field:SerializeField] public Alterations Alteration { get; private set; }
        [field:SerializeField] public int Amount { get; private set; }
        [field:SerializeField] public bool FlatValue { get; private set; }
    }
}