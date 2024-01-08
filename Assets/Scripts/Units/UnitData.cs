using System;
using System.Collections.Generic;
using Scriptable_Objects.Spells___Effects;
using Scriptable_Objects.Unit;
using UnityEngine;

namespace Units
{
    public abstract class UnitData : UnitInfo
    {
        public static event Action OnPlayerLifeUpdated;
        protected UnitSO unitSo;

        protected int currentHp;
        protected int currentMp;
        
        private List<Alterations> alterations = new List<Alterations>();
        private int _currentHp;
        private int _currentMp;
        private int _maxHp;
        private int _maxMp;
        private Sprite _sprite;
        private int _currentHp1;
        private int _currentMp1;
        private int _maxHp1;
        private int _maxMp1;


        public UnitData(UnitSO unitSo)
        {
            this.unitSo = unitSo;
            Init();
        }

        protected int GetBaseStat(Stats stat)
        {
            switch (stat)
            {
                case Stats.Strength:
                    return unitSo.Strength;
                case Stats.Agility:
                    return unitSo.Agility;
                case Stats.Stamina:
                    return unitSo.Stamina;
                case Stats.Magic:
                    return unitSo.Magic;
                case Stats.Attack:
                    return unitSo.Attack;
                case Stats.Defence:
                    return unitSo.Defence;
                case Stats.MagicDefence:
                    return unitSo.MagicDefence;
                case Stats.Evasion:
                    return unitSo.Evasion;
                case Stats.MagicEvasion:
                    return unitSo.MagicEvasion;
                default:
                    return 0;
            }
        }
        
        protected virtual void Init()
        {}

        public virtual int CurrentHp
        {
            get => currentHp;
        }
        
        public virtual string GetName()
        {
            return unitSo.Name;
        }

        public virtual int CurrentMp
        {
            get => currentMp;
        }

        public virtual int MaxHp
        {
            get => 0;
        }

        public virtual int MaxMp
        {
            get => 0;
        }

        public virtual int Strength
        {
            get => unitSo.Strength;
        }

        public virtual int Agility
        {
            get => unitSo.Agility;
        }

        public virtual int Stamina
        {
            get => unitSo.Stamina;
        }

        public virtual int Magic
        {
            get => unitSo.Magic;
        }

        public virtual int Attack
        {
            get => unitSo.Attack;
        }
        
        public virtual int Strenght2
        {
            get {
                int strenght2 = unitSo.Strength * 2;
                return strenght2 > 128 ? 128 : strenght2;
            }
        }
        
        public virtual int Level
        {
            get => 1;
        }
        
        public virtual int Attack2
        {
            get => this.Attack + this.Strenght2;
        }

        public virtual int Damage
        {
            get => Attack + ((Level * Level * Attack2) / 256) * (3 / 2);
        }
        
        

        public virtual int Defence
        {
            get => unitSo.Defence;
        }

        public virtual int MagicDefence
        {
            get => unitSo.MagicDefence;
        }

        public virtual int Evasion
        {
            get => unitSo.Evasion;
        }

        public virtual int MagicEvasion
        {
            get => unitSo.MagicEvasion;
        }

        public virtual int Resistance(Elements element)
        {
            ElementResistance resist = unitSo.Resistances.Find(resistance => resistance.Element == element);
            if (resist == null) return 100;
            var baseResist = resist.Resistance;
            //TODO: Check alteration for resistances
            
            
            int resistance = baseResist;
            
            return resistance;
        }

        public virtual bool IsImmuneTo(Alterations alteration)
        {
            return unitSo.AlterationImmunity.Exists( immunity => immunity.Alteration == alteration && immunity.Immunity);
        }

        public virtual Sprite Sprite
        {
            get => unitSo.Sprite;
        }

        public int TakeDamage(int damage)
        {
            currentHp -= damage;
            if (currentHp < 0)
            {
                var amount = Math.Abs(currentHp);
                currentHp = 0;
                return amount;
            }
            if (currentHp > MaxHp)
            {
                var amount = currentHp - MaxHp;
                currentHp = MaxHp;
                return amount;
            }

            return 0;
        }
        
        public int RegenMpDamage(int regen)
        {
            currentMp += regen;
            if (currentMp < 0)
            {
                var amount = Math.Abs(currentMp);
                currentMp = 0;
                return amount;
            }
            if (currentMp > MaxMp)
            {
                var amount = currentMp - MaxMp;
                currentMp = MaxMp;
                return amount;
            }

            return 0;
        }
        
        
        
        
        public bool IsAflictedBy(Alterations alteration)
        {
            return alterations.Contains(alteration);
        }
        
        public void AddAlteration(Alterations alteration, bool ignoreImmunity)
        {
            if (IsAflictedBy(alteration)) return;
            if (!ignoreImmunity && IsImmuneTo(alteration)) return;
            
            alterations.Add(alteration);
        }
        
        public void RemoveAlteration(Alterations alteration, bool ignoreImmunity)
        {
            if (!IsAflictedBy(alteration)) return;
            if (!ignoreImmunity && IsImmuneTo(alteration)) return;
            
            alterations.Remove(alteration);
        }
        
        public void HPNeedToBeUpdated()
        {
            OnPlayerLifeUpdated?.Invoke();
        }
    }
    
    public interface UnitInfo
    {
        public int CurrentHp { get; }
        public int CurrentMp { get; }
        
        public int MaxHp { get; }
        public int MaxMp { get; }
        
        public int Strength { get; }
        public int Agility { get; }
        public int Stamina { get; }
        public int Magic { get; }
        
        public int Attack { get; }
        
        public int Defence { get; }
        public int MagicDefence { get; }
        public int Evasion { get; }
        public int MagicEvasion { get; }
        
        public int Resistance(Elements element);
        public bool IsImmuneTo(Alterations alteration);
        
        public Sprite Sprite { get; }
    }
}