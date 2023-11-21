using System.Collections.Generic;
using Scriptable_Objects.Spells___Effects;
using UnityEngine;

namespace Scriptable_Objects.Items
{
    [CreateAssetMenu(fileName = "new UsableItem", menuName = "ScriptableObjects/Items/UsableItemSo", order = 0)]
    public class UsableItemSo : ItemSO
    {
        [field:Header("---UsableItemSO---")]
        [field:SerializeField] public List<EffectSO> Effects { get; private set; }
        
    }
}