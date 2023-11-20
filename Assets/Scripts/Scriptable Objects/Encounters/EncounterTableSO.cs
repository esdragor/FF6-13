using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable_Objects.Encounters
{
    [CreateAssetMenu(fileName = "new EncounterTable", menuName = "ScriptableObjects/Encounters/EncounterTableSO", order = 0)]
    public class EncounterTableSO : ScriptableObject
    {
        [field:Header("---EncounterTableSO---")]
        [field:SerializeField] public Location Location { get; private set; }
        [field:SerializeField] public List<EncounterChance> Encounters { get; private set; }
    }
    
    [Serializable] public class EncounterChance
    {
        [field:SerializeField] public EncounterSO Encounter { get; private set; }
        [field:SerializeField] public int EncounterWeight { get; private set; }
    }
    
    public enum Location
    {
        Nashe,
        
        
        TODO, //TODO: add locations
    }
}