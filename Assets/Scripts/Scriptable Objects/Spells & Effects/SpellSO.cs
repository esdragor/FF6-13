using System.Collections.Generic;
using UnityEngine;

namespace Scriptable_Objects.Spells___Effects
{
    [CreateAssetMenu(fileName = "New Spell", menuName = "ScriptableObjects/Spell/SpellSO", order = 0)]
    public class SpellSO : ScriptableObject
    {
        [field:Header("---SpellSO---")]
        [field:SerializeField] public string Name { get; private set; } 
        [field:SerializeField] public string Description { get; private set; } //TODO : might do the description automatically
        [field:SerializeField] public int MpCost { get; private set; }
        [field:SerializeField] public int ApCost { get; private set; }
        [field:SerializeField] public List<EffectSO> SpellEffects { get; private set; }
    }
}