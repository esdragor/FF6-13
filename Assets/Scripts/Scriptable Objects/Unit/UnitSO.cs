using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scriptable_Objects.Unit
{
    public abstract class UnitSO : ScriptableObject
    {
        [field:Header("---UnitSO---")]
        
        [field:SerializeField] public string Name { get; private set; }
        
        [field:Header("Stats")]
        [field:SerializeField] public int Hp { get; private set; }
        [field:SerializeField] public int Strength { get; private set; }
        [field:SerializeField] public int Agility { get; private set; }
        [field:SerializeField] public int Stamina { get; private set; }
        [field:SerializeField] public int Magic { get; private set; }
        [field:Space]
        [field:SerializeField] public int Attack { get; private set; }
        [field:Space]
        [field:SerializeField] public int Defence { get; private set; }
        [field:SerializeField] public int MagicDefence { get; private set; }
        [field:SerializeField] public int Evasion { get; private set; }
        [field:SerializeField] public int MagicEvasion { get; private set; }
        
        [field:Space]
        [field:Header("Resistances")]
        [field:SerializeField] public List<ElementResistance> Resistances { get; private set; }
        [field:SerializeField] public List<AlterationImmunity> AlterationImmunity { get; private set; } //Change to ?
        [field:Space]
        [field:Header("Misc")]
        [field:SerializeField] public Sprite Sprite { get; private set; }

        
        public UnitSOInstance CreateInstance()
        {
            return new UnitSOInstance(this);
        }
        
        
        [ContextMenu("Init All")]
        public void InitAll()
        {
            InitAllResistances();
            InitAllImmunity();
        }
        
        [ContextMenu("Init All Resistances")]
        public void InitAllResistances()
        {
            Resistances = new List<ElementResistance>();
            
            for (int i = 0; i < Enum.GetValues(typeof(Elements)).Length-1; i++)
            {
                Resistances.Add(new ElementResistance((Elements) i, Elements.Heal == (Elements) i ? -100 : 100));
            }
        }

        [ContextMenu("Init All Immunity")]
        public void InitAllImmunity()
        {
            AlterationImmunity = new List<AlterationImmunity>();

            for (int i = 0; i < Enum.GetValues(typeof(Alterations)).Length; i++)
            {
                AlterationImmunity.Add(new AlterationImmunity((Alterations)i, false));
            }
        }
    }
    
    public class UnitSOInstance
    {
        public UnitSO So { get;}
        public int currentHp { get; set; }
        
        public UnitSOInstance(UnitSO info)
        {
            So = info;
            currentHp = info.Hp;
        }
    }
    
    [Serializable] public class ElementResistance
    {
        [field:SerializeField] public string name { get; private set; }
        [field:SerializeField] public Elements Element { get; private set; }
        [field:SerializeField] public int Resistance { get; private set; }
        
        public ElementResistance(Elements element, int resistance)
        {
            Element = element;
            Resistance = resistance;
            name = $"{element}";
        }
    }
    
    [Serializable] public class AlterationImmunity
    {
        [field:SerializeField] public string name { get; private set; }
        [field:SerializeField] public Alterations Alteration { get; private set; }
        [field:SerializeField] public bool Immunity { get; private set; }
        
        public AlterationImmunity(Alterations alterations, bool immunity)
        {
            Alteration = alterations;
            Immunity = immunity;
            name = $"{alterations}";
        }
    }
    


    
    
    // TODO: move theses enums outside of here
    public enum Stats
    {
        Strength,
        Agility,
        Stamina,
        Magic,
        Attack,
        HitRate,
        Defence,
        MagicDefence,
        Evasion,
        MagicEvasion
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
        AllDamage,
        Death
    }
    
    /*
        - -100% = absorption,
        - 0% = annulation,
        - 200% = faiblesse
     */
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
        None
    }
}
