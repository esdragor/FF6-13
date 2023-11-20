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
        [field:SerializeField] public GrowthRatesSO GrowthTable { get; private set; }
        
        [field:Space]
        [field:Header("EquipmentAvailable")]
        [field:SerializeField] public List<EquipableType> RightHandTypes { get; private set; }
        [field:SerializeField] public List<EquipableType> LeftHandTypes { get; private set; }
        [field:SerializeField] public List<EquipableType> HeadTypes { get; private set; }
        [field:SerializeField] public List<EquipableType> BodyTypes { get; private set; }
        
        public PlayerCharacterInfoInstance CreateInstance(int currentLevel)
        {
            return new PlayerCharacterInfoInstance(this, currentLevel);
        }
    }
    
    public class PlayerCharacterInfoInstance
    {
        public PlayerCharactersSO So { get;}
        public int currentHp { get; private set; }
        public int currentMp { get; private set; }
        public int currentLvl { get; private set; }
        
        [field:SerializeField] public List<EquipableType> currentRightHandTypes { get; private set; }
        [field:SerializeField] public List<EquipableType> currentLeftHandTypes { get; private set; }
        [field:SerializeField] public List<EquipableType> currentHeadTypes { get; private set; }
        [field:SerializeField] public List<EquipableType> currentBodyTypes { get; private set; }
        
        public PlayerCharacterInfoInstance(PlayerCharactersSO info, int currentLevel)
        {
            So = info;
            currentLvl = currentLevel;
            currentHp = info.InitialHp + info.GrowthTable.GrowthRates[currentLevel].Health;
            currentMp = info.InitialMp + info.GrowthTable.GrowthRates[currentLevel].Mp;
            currentRightHandTypes = info.RightHandTypes;
            currentLeftHandTypes = info.LeftHandTypes;
            currentHeadTypes = info.HeadTypes;
            currentBodyTypes = info.BodyTypes;
        }
        
        public void AddRightHandType(EquipableType type)
        {
            currentRightHandTypes.Add(type);
        }
        
        public void AddLeftHandType(EquipableType type)
        {
            currentLeftHandTypes.Add(type);
        }
        
        public void AddHeadType(EquipableType type)
        {
            currentHeadTypes.Add(type);
        }
        
        public void AddBodyType(EquipableType type)
        {
            currentBodyTypes.Add(type);
        }
        
        public void LevelUp()
        {
            currentLvl++;
        }
    }
}