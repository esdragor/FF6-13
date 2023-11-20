using Scriptable_Objects.Items;
using Scriptable_Objects.Spells___Effects;
using Scriptable_Objects.Unit;

namespace Items
{
    public class EquipableData : Items.ItemData
    {
        private EquipableSO equipableSo => (EquipableSO)itemSO;
        
        public EquipableData(ItemSO itemSo) : base(itemSo)
        {}

        public virtual int getStat(Stats stat, bool flat)
        {
            /* Return the bonuses of the stat provided by the item */
            var value = 0;

            foreach (var itemEffect in equipableSo.ItemEffects)
            {
                if (itemEffect as StatChangeOS != null)
                {
                    var statChange = (StatChangeOS) itemEffect;
                    if (statChange.Stat == stat && statChange.FlatValue == flat)
                    {
                        value += statChange.Amount;
                    }
                }
            }

            return value;
        }
    }
}