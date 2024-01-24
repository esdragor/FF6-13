using System.Collections.Generic;
using UnityEngine;

namespace Scriptable_Objects.Interaction.IneractionActions
{
    [CreateAssetMenu(fileName = "new InteractionTpCamSO", menuName = "ScriptableObjects/InteractionAction/InteractionTpCamSO", order = 0)]
    public class InteractionTpCamSO : InteractionActionSO
    {
        [field: SerializeField] public Vector2 TargetPos { get; set; }
        [field:SerializeField] public List<Direction> BlockedDirections { get; set; }
    }
}