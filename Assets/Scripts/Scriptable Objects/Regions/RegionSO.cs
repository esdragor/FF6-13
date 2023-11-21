using System.Collections.Generic;
using Scriptable_Objects.Encounters;
using UnityEngine;

namespace Scriptable_Objects.Regions
{
    [CreateAssetMenu(fileName = "new Region", menuName = "ScriptableObjects/Region", order = 0)]
    public class RegionSO : ScriptableObject
    {
        [field:Header("---RegionSO---")]
        [field:SerializeField] public Location Name { get; private set; }
        
        [field:Header("EncounterTables")]
        [field:SerializeField] public List<EncounterTableSO> EncounterTables { get; private set; }

        [field:Header("Audio")]
        [field:SerializeField] public List<AudioClip> Ambiance { get; private set; }
        [field:SerializeField] public List<AudioClip> BattleSounds { get; private set; }
        
        [field:Header("Backgrounds")]
        [field:SerializeField] public List<Sprite> Backgrounds { get; private set; }
    }
}