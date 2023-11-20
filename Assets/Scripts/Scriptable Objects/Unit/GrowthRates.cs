using System.Collections.Generic;
using UnityEngine;

namespace Scriptable_Objects.Unit
{
    [CreateAssetMenu(fileName = "Growth rates", menuName = "ScriptableObjects/Units/GrowthSO", order = 0)]
    public class GrowthRates : ScriptableObject
    {
        [field:Header("Growth Rates")]
        [field:SerializeField] public List<int> HealthGrowth { get; private set; }
        [field:SerializeField] public List<int> MpGrowth { get; private set; }
        [field:SerializeField] public List<int> Xp { get; private set; }
    }
}