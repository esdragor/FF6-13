using System;
using Scriptable_Objects.Unit;
using UnityEngine;

namespace Scriptable_Objects.Spells___Effects
{
    [CreateAssetMenu(fileName = "new IgnoreAlteration", menuName = "ScriptableObjects/Effects/IgnoreAlterationSO", order = 0)]
    [Serializable] public class IgnoreAlterationSO : EffectSO
    {
        [field:SerializeField] public Alterations Alteration { get; private set; }
        [field:SerializeField, Tooltip("True mean ignore for spell, is immune for items")] public bool IsPositive { get; set; }
    }
}