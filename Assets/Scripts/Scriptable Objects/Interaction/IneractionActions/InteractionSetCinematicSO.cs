using UnityEngine;

namespace Scriptable_Objects.Interaction.IneractionActions
{
    [CreateAssetMenu(fileName = "New InteractionSetCinematic", menuName = "ScriptableObjects/InteractionAction/InteractionSetCinematicSO", order = 0)]
    public class InteractionSetCinematicSO : InteractionActionSO
    {
        [field: SerializeField] public bool SetCinematic { get; set; }
    }
}