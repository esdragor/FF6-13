using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable_Objects.Unit
{
    [CreateAssetMenu(fileName = "Growth rates", menuName = "ScriptableObjects/Units/GrowthSO", order = 0)]
    public class GrowthRatesSO : ScriptableObject
    {
        [Serializable]
        public struct GrowthRate
        {
            [field:SerializeField] public int Health{ get; private set; }
            [field:SerializeField] public int Mp{ get; private set; }
            [field:SerializeField] public int Xp { get; private set; }
        }
        
        [SerializeField] private List<GrowthRate> growthRates = new List<GrowthRate>();
        public IReadOnlyList<GrowthRate> GrowthRates => growthRates;
    }
}