using System;
using System.Collections.Generic;
using Scriptable_Objects.Items;
using Scriptable_Objects.Spells___Effects;
using UnityEngine;

namespace Scriptable_Objects.Unit
{
    [CreateAssetMenu(fileName = "new PlayerCharactersSO", menuName = "ScriptableObjects/Units/PlayerCharactersSO", order = 0)]
    public class PlayerCharactersSO : UnitSO
    {
        [field:Header("---PlayerCharactersSO---")]
        [field:SerializeField] public int InitialHp { get; private set; }
        [field:SerializeField] public int InitialMp { get; private set; }
        [field:SerializeField] public int EscapeRate { get; private set; }
        [field:SerializeField] public GrowthRatesSO GrowthTable { get; private set; }
        
        [field:Space]
        [field:Header("EquipmentAvailable")]
        [field:SerializeField] public List<EquipableType> RightHandTypes { get; private set; }
        [field:SerializeField] public List<EquipableType> LeftHandTypes { get; private set; }
        [field:SerializeField] public List<EquipableType> HeadTypes { get; private set; }
        [field:SerializeField] public List<EquipableType> BodyTypes { get; private set; }
        
        [field:Space]
        [field:Header("Spells")]
        [field:SerializeField] public List<SpellUnlocks> SpellUnlocksList { get; private set; }
        
        
        [Serializable] public struct SpellUnlocks
        {
            public SpellUnlocks(SpellSO spell, int level)
            {
                Spell = spell;
                Level = level;
            }

            [field:SerializeField] public SpellSO Spell { get; private set; }
            [field:SerializeField] public int Level { get; private set; }
        }
    }
}