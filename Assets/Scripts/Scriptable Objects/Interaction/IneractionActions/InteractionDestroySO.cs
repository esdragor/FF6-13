using UnityEngine;

namespace Scriptable_Objects.Interaction.IneractionActions
{
    [CreateAssetMenu(fileName = "New InteractionDestroy", menuName = "ScriptableObjects/InteractionAction/InteractionDestroy", order = 0)]
    public class InteractionDestroySO : InteractionActionSO
    {
        [field:SerializeField] public string DestroyId { get; set; }
    }
}