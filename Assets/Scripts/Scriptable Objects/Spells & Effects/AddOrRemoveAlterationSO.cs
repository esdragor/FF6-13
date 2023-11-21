using System;
using Scriptable_Objects.Unit;
using UnityEngine;

namespace Scriptable_Objects.Spells___Effects
{
    [CreateAssetMenu(fileName = "new AddOrRemoveAlteration", menuName = "ScriptableObjects/Effects/AddOrRemoveAlterationSO", order = 0)]
    [Serializable] public class AddOrRemoveAlterationSO : EffectSO
    {
        [field:SerializeField] public Alterations Alteration { get; private set; }
        [field:SerializeField] public bool Remove { get; private set; }
        [field:SerializeField] public bool SingleTarget { get; private set; }
    }
}