using System;
using Scriptable_Objects.Unit;
using UnityEngine;

namespace Scriptable_Objects.Spells___Effects
{
    [CreateAssetMenu(fileName = "new Stat Change", menuName = "ScriptableObjects/Effects/StatChangeSO", order = 0)]
    [Serializable] public class StatChangeOS : EffectSO
    {
        [field:SerializeField] public Stats Stat { get; private set; }
        [field:SerializeField] public int Amount { get; private set; }
        [field:SerializeField] public bool FlatValue { get; private set; }
        
        public int GetStatChanged(bool flatValue)
        {
            if (flatValue != FlatValue) return 0;
            
            return Amount;
        }
    }
}