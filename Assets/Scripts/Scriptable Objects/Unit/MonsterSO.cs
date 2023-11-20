using UnityEngine;

namespace Scriptable_Objects.Unit
{
    [CreateAssetMenu(fileName = "new MonsterSO", menuName = "ScriptableObjects/Units/MonsterSO", order = 0)]
    public class MonsterSO : UnitSO
    {
        [field:Header("---PlayerCharactersSO---")]
        [field:SerializeField] public int XpReward { get; private set; }
        [field:SerializeField] public int GilReward { get; private set; }
        [field:Space]
        // Steal item
        // Drop item
        // Morph items
        [field:SerializeField] public float MorphFailChance { get; private set; }
    }
}