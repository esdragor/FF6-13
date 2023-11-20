using System.Collections.Generic;
using UnityEngine;

namespace Scriptable_Objects.Unit
{
    public abstract class UnitSO : ScriptableObject
    {
        [field:Header("---UnitSO---")]
        
        [field:Header("Stats")]
        [field:SerializeField] public int Strength { get; private set; }
        [field:SerializeField] public int Agility { get; private set; }
        [field:SerializeField] public int Stamina { get; private set; }
        [field:SerializeField] public int Magic { get; private set; }
        [field:Space]
        [field:SerializeField] public int Attack { get; private set; }
        [field:SerializeField] public int HitRate { get; private set; }
        [field:Space]
        [field:SerializeField] public int Defence { get; private set; }
        [field:SerializeField] public int MagicDefence { get; private set; }
        [field:SerializeField] public int Evasion { get; private set; }
        [field:SerializeField] public int MagicEvasion { get; private set; }
        
        [field:Space]
        [field:Header("Resistances")]
        [field:SerializeField] public Dictionary<Elements, int> Resistances { get; private set; }
        [field:SerializeField] public Dictionary<Alterations, bool> AlterationImmunity { get; private set; } //Change to ?
    }


    public enum Alterations
    {
        Darkness,
        Zombie,
        Poison,
        MagiTek,
        Clear,
        Imp,
        Petrify,
        Fatal,
        Condemned,
        Critical,
        Image,
        Mute,
        Berserk,
        Confuse,
        Sap,
        Psyche,
        Float,
        Regen,
        Slow,
        Haste,
        Stop,
        Shell,
        Safe,
        Reflect,
        Suplex,
        Scan,
        Sketch,
        Control,
        Fractional,
        AllDamage
    }
    
    public enum Elements
    {
        Water,
        Fire,
        Lightning,
        Ice,
        InstantDeath,
        Poison,
        Holy,
        Heal,
        Earth,
        Wind,
    }
}
