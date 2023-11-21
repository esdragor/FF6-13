using System.Collections.Generic;
using Scriptable_Objects.Spells___Effects;
using UnityEngine;

namespace Scriptable_Objects.Items
{
    public abstract class EquipableSO : ItemSO
    {
        [field:Header("---EquipableSO---")]
        [field:SerializeField] public EquipableType EquipableType { get; protected set; }
        [field:SerializeField] public List<EffectSO> ItemEffects { get; protected set; }
    }
    
    public enum EquipableType
    {
        // Armors
        UniversalShields,
        HeavyShields,
        Hats,
        FemaleHats,
        Helmets,
        LightArmor,
        Robes,
        FemaleClothing,
        Costumes,
        HeavyArmor,
        
        // Weapons
        Daggers,
        Swords,
        Spears,
        NinjaDaggers,
        Katanas,
        Rods,
        Claws,
        Maces,
        ThrowingWeapons,
        GamblerItems,
        Brushes,
        Shurikens,
        Scrolls,
        Tools,
        
        // Relics
        Relics,
        
        Others,

        
    }
    
}