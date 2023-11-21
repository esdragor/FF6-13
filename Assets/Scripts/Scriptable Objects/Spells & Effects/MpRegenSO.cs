using System;
using UnityEngine;

namespace Scriptable_Objects.Spells___Effects
{
    [CreateAssetMenu(fileName = "new MpRegen", menuName = "ScriptableObjects/Effects/MpRegenSO", order = 0)]
    [Serializable] public class MpRegenSO : EffectSO
    {
        [field:SerializeField] public int Regen { get; private set; }
        [field:SerializeField] public bool FlatValue { get; private set; }
        [field:SerializeField] public bool SingleTarget { get; private set; }
        
    }
}