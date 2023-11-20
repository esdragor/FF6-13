using System.Collections.Generic;
using Scriptable_Objects.Items;
using UnityEngine;

namespace Scriptable_Objects.Unit
{
    [CreateAssetMenu(fileName = "new PlayerCharactersSO", menuName = "ScriptableObjects/Units/PlayerCharactersSO", order = 0)]
    public class PlayerCharactersSO : UnitSO
    {
        [field:Header("---PlayerCharactersSO---")]
        [field:SerializeField] public int InitialHp { get; private set; }
        [field:SerializeField] public int InitialMp { get; private set; }
        [field:SerializeField] public int EscapeRate { get; private set; }
        
        [field:Space]
        [field:Header("EquipmentAvailable")]
        [field:SerializeField] public List<EquipableType> RightHandTypes { get; private set; }
        [field:SerializeField] public List<EquipableType> LeftHandTypes { get; private set; }
        [field:SerializeField] public List<EquipableType> HeadTypes { get; private set; }
        [field:SerializeField] public List<EquipableType> BodyTypes { get; private set; }
    }
}