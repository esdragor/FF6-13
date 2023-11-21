using System.Collections.Generic;
using Scriptable_Objects.Unit;
using UnityEngine;

namespace Scriptable_Objects.Items
{
    [CreateAssetMenu(fileName = "New Relic", menuName = "ScriptableObjects/Items/RelicSO", order = 0)]
    public class RelicSO : EquipableSO
    {
        [field:Header("---RelicSO---")]
        [field:SerializeField] private List<PlayerCharactersSO> UsableBy { get; set; }

        
        public bool CanBeEquipedBy(PlayerCharactersSO playerCharactersSo)
        {
            if (UsableBy.Count == 0) return true;
            
            return UsableBy.Contains(playerCharactersSo);
        }
    }
}