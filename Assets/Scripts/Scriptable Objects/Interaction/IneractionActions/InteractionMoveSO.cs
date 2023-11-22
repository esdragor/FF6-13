using System.Collections.Generic;
using UnityEngine;

namespace Scriptable_Objects.Interaction.IneractionActions
{
    [CreateAssetMenu(fileName = "New InteractionMove", menuName = "ScriptableObjects/InteractionAction/InteractionMoveSO", order = 0)]
    public class InteractionMoveSO : InteractionActionSO
    {
        [field:SerializeField] public string UnitId { get; set; }
        [field:SerializeField] public List<Direction> Directions { get; set; }
        [field:SerializeField] public bool WaitTillFinish { get; set; }
        
    }
}