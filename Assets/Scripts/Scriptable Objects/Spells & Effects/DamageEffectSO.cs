using System;
using Scriptable_Objects.Unit;
using UnityEngine;

namespace Scriptable_Objects.Spells___Effects
{
    [CreateAssetMenu(fileName = "new DamageEffect", menuName = "ScriptableObjects/Effects/DamageEffectSO", order = 0)]
    [Serializable] public class DamageEffectSO : EffectSO
    {
        [field:SerializeField] public Elements Element { get; private set; }
        [field:SerializeField] public int Damage { get; private set; }
        [field:SerializeField] public bool FlatValue { get; private set; }
        [field:SerializeField] public bool SingleTarget { get; private set; }
    }
}