using Scriptable_Objects.Unit;
using UnityEngine;

namespace Scriptable_Objects.Interaction.IneractionActions
{
    [CreateAssetMenu(fileName = "New IneractionSpawn", menuName = "ScriptableObjects/InteractionAction/IneractionSpawnSO", order = 0)]
    public class IneractionSpawnSO : InteractionActionSO
    {
        [field:SerializeField] public MonsterSO UnitToSpawn { get; set; } // Change to another type
        [field:SerializeField] public string StoreUnitAsId { get; set; }
        [field:SerializeField] public Vector2 SpawnPos { get; set; }
    }
}