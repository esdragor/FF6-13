using System.Collections.Generic;
using Scriptable_Objects.Items;
using UnityEngine;

namespace Scriptable_Objects.Shops
{
    [CreateAssetMenu(fileName = "New Shop", menuName = "ScriptableObjects/Shop", order = 0)]
    public class ShopSo : ScriptableObject
    {
        [field:Header("---ShopSO---")] 
        [field:SerializeField] public string Name { get; private set; }
        [field:SerializeField] public List<ItemSO> ItemsAvailable { get; private set; }
    }
}