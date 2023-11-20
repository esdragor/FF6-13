using System;
using System.Collections.Generic;
using Scriptable_Objects.Unit;
using UnityEngine;

namespace Scriptable_Objects.Encounters
{
    [CreateAssetMenu(fileName = "new Encounter", menuName = "ScriptableObjects/Encounters/EncounterSO", order = 0)]
    public class EncounterSO : ScriptableObject
    {
        [field:Header("---EncounterSO---")]
        [field:SerializeField] public List<EncounterData> Monsters { get; private set; }
    }
    
    [Serializable] public class EncounterData
    {
        [field:SerializeField] public MonsterSO Monster { get; private set; }
        [field:SerializeField] public int MonsterCount { get; private set; }
    }
}