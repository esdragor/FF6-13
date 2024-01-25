using System.Collections.Generic;
using Items;
using Scriptable_Objects.Items;
using Scriptable_Objects.Spells___Effects;
using Scriptable_Objects.Unit;

namespace Units
{
    public class PlayerCharacterData : UnitData
    {
        public int Lvl { get; private set; }
        public int xp { get; private set; }

        public EquipableData RightHand { get; private set; } = null;
        public EquipableData LeftHand { get; private set; } = null;
        public EquipableData Head { get; private set; } = null;
        public EquipableData Body { get; private set; } = null;
        public EquipableData Accessory1 { get; private set; } = null;
        public EquipableData Accessory2 { get; private set; } = null;

        private bool itemEquippedRightHand => RightHand != null;
        private bool itemEquippedLeftHand => LeftHand != null;
        private bool itemEquippedHead => Head != null;
        private bool itemEquippedBody => Body != null;
        private bool itemEquippedAccessory1 => Accessory1 != null;
        private bool itemEquippedAccessory2 => Accessory2 != null;

        public PlayerCharactersSO PlayerCharactersSo => (PlayerCharactersSO)UnitSo;
        private GrowthRatesSO growthRatesSo;

        public PlayerCharacterData(PlayerCharactersSO unitSo, int lvl) : base(unitSo)
        {
            Lvl = lvl;
            xp = 0;
            growthRatesSo = unitSo.GrowthTable;

            Init();
        }

        protected override void Init()
        {
            currentHp = MaxHp;
            currentMp = MaxMp;
        }

        public override int MaxHp
        {
            get
            {
                if (Lvl <= 1) return PlayerCharactersSo.InitialHp;
                return PlayerCharactersSo.InitialHp + growthRatesSo.GrowthRates[Lvl - 2].Health;
            }
        }

        public override int MaxMp
        {
            get
            {
                if (Lvl <= 1) return PlayerCharactersSo.InitialMp;
                return PlayerCharactersSo.InitialMp + growthRatesSo.GrowthRates[Lvl - 2].Mp;
            }
        }

        private (int, int) GetItemsStats(Stats stat)
        {
            return (GetItemsStats(stat, false), GetItemsStats(stat, true));
        }

        private int GetItemsStats(Stats stat, bool flat)
        {
            var val = 0;

            if (itemEquippedRightHand)
            {
                val += RightHand.getStat(stat, flat);
            }

            if (itemEquippedLeftHand)
            {
                val += LeftHand.getStat(stat, flat);
            }

            if (itemEquippedHead)
            {
                val += Head.getStat(stat, flat);
            }

            if (itemEquippedBody)
            {
                val += Body.getStat(stat, flat);
            }

            if (itemEquippedAccessory1)
            {
                val += Accessory1.getStat(stat, flat);
            }

            if (itemEquippedAccessory2)
            {
                val += Accessory2.getStat(stat, flat);
            }

            return val;
        }

        public override int Strength
        {
            get
            {
                var val = GetBaseStat(Stats.Strength);
                var itemsStat = GetItemsStats(Stats.Strength);
                var alterationsStat = (0, 0); //TODO

                return val + itemsStat.Item1 + alterationsStat.Item1 * (1 + itemsStat.Item2 + alterationsStat.Item2);
            }
        }

        public override int Agility
        {
            get
            {
                var val = GetBaseStat(Stats.Agility);
                var itemsStat = GetItemsStats(Stats.Agility);
                var alterationsStat = (0, 0); //TODO

                return val + itemsStat.Item1 + alterationsStat.Item1 * (1 + itemsStat.Item2 + alterationsStat.Item2);
            }
        }

        //TODO: There is a calcule on Stamina that is not on the other stats, check if it is correct
        public override int Stamina
        {
            get
            {
                var val = GetBaseStat(Stats.Stamina);
                var itemsStat = GetItemsStats(Stats.Stamina);
                var alterationsStat = (0, 0); //TODO

                return val + itemsStat.Item1 + alterationsStat.Item1 * (1 + itemsStat.Item2 + alterationsStat.Item2);
            }
        }

        public override int Magic
        {
            get
            {
                var val = GetBaseStat(Stats.Magic);
                var itemsStat = GetItemsStats(Stats.Magic);
                var alterationsStat = (0, 0); //TODO

                return val + itemsStat.Item1 + alterationsStat.Item1 * (1 + itemsStat.Item2 + alterationsStat.Item2);
            }
        }

        public List<SpellSO> getAllSpells()
        {
            return PlayerCharactersSo.SpellUnlocksList.FindAll(spellUnlock => spellUnlock.Level <= Lvl)
                .ConvertAll(spellUnlock => spellUnlock.Spell);
        }
        public List<UsableItemSo> getAllItems()
        {
            return PlayerCharactersSo.Inventory;
        }
        
        public int getIndexOfSpell(SpellSO spell)
        {
            return PlayerCharactersSo.SpellUnlocksList.FindIndex(spellUnlock => spellUnlock.Spell == spell);
        }
        
        public int getIndexOfItem(ItemSO item)
        {
            return PlayerCharactersSo.Inventory.FindIndex(itemSelected => itemSelected == item);
        }
        
        public override int Level => Lvl;

        public override int Attack
        {
            get
            {
                var val = GetBaseStat(Stats.Attack);
                var itemsStat = GetItemsStats(Stats.Attack);
                var alterationsStat = (0, 0); //TODO

                return val + itemsStat.Item1 + alterationsStat.Item1 * (1 + itemsStat.Item2 + alterationsStat.Item2);
            }
        }

        public override int Defence
        {
            get
            {
                var val = GetBaseStat(Stats.Defence);
                var itemsStat = GetItemsStats(Stats.Defence);
                var alterationsStat = (0, 0); //TODO

                return val + itemsStat.Item1 + alterationsStat.Item1 * (1 + itemsStat.Item2 + alterationsStat.Item2);
            }
        }

        public override int MagicDefence
        {
            get
            {
                var val = GetBaseStat(Stats.MagicDefence);
                var itemsStat = GetItemsStats(Stats.MagicDefence);
                var alterationsStat = (0, 0); //TODO

                return val + itemsStat.Item1 + alterationsStat.Item1 * (1 + itemsStat.Item2 + alterationsStat.Item2);
            }
        }

        public override int Evasion
        {
            get
            {
                var val = GetBaseStat(Stats.Evasion);
                var itemsStat = GetItemsStats(Stats.Evasion);
                var alterationsStat = (0, 0); //TODO

                return val + itemsStat.Item1 + alterationsStat.Item1 * (1 + itemsStat.Item2 + alterationsStat.Item2);
            }
        }

        public override int MagicEvasion
        {
            get
            {
                var val = GetBaseStat(Stats.MagicEvasion);
                var itemsStat = GetItemsStats(Stats.MagicEvasion);
                var alterationsStat = (0, 0); //TODO

                return val + itemsStat.Item1 + alterationsStat.Item1 * (1 + itemsStat.Item2 + alterationsStat.Item2);
            }
        }

        public override bool IsImmuneTo(Alterations alteration)
        {
            //TODO Check if the unit is immune to the alteration in his equipment
            return UnitSo.AlterationImmunity.Exists(immunity => immunity.Alteration == alteration && immunity.Immunity);
        }
        
        public bool GainXp(int amount)
        {
            bool levelUp = false;
            xp += amount;
            while (xp >= PlayerCharactersSo.GrowthTable.GrowthRates[Lvl - 1].Xp)
            {
                Lvl++;
                levelUp = true;
            }
            return levelUp;
        }
        
        public int GetXpToNextLevel()
        {
            return PlayerCharactersSo.GrowthTable.GrowthRates[Lvl - 1].Xp - xp;
        }
        
    }
}