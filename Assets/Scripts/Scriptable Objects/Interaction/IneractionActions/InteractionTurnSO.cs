using UnityEngine;

namespace Scriptable_Objects.Interaction.IneractionActions
{
    [CreateAssetMenu(fileName = "New InteractionTurn", menuName = "ScriptableObjects/InteractionAction/InteractionTurnGO", order = 0)]
    public class InteractionTurnSO : InteractionActionSO
    {
        [field:SerializeField] public string UnitId { get; set; }
        [field:SerializeField] public Direction Direction { get; set; }
    }
}