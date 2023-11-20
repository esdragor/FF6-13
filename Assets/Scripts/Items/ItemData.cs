using Scriptable_Objects.Items;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Items
{
    public class ItemData
    {
        public int CurrentQuantity { get; private set; }
        
        public virtual string Name { get => itemSO.Name; }
        public virtual string Description { get => itemSO.Description; }
        public int MaxQuantity { get => itemSO.MaxQuantity; }
        public virtual Sprite Sprite { get => itemSO.Sprite; }
        public virtual int Price { get => itemSO.Price; }
        
        protected ItemSO itemSO;
        
        public ItemData(ItemSO itemSo)
        {
            this.itemSO = itemSo;
        }

        
        
        public int AddQuantity(int quantity)
        {
            if (quantity <= 0) return 0;
            
            CurrentQuantity += quantity;

            if (CurrentQuantity > MaxQuantity)
            {
                var overflow = CurrentQuantity - MaxQuantity;
                CurrentQuantity = MaxQuantity;
                return overflow;
            }
            
            return 0;
        }
        
        public int RemoveQuantity(int quantity)
        {
            if (quantity >= 0) return 0;
            
            CurrentQuantity -= quantity;
            
            if (CurrentQuantity < 0)
            {
                var overflow = CurrentQuantity;
                CurrentQuantity = 0;
                return overflow;
            }
            
            return 0;
        }
    }
}