using UnityEngine;

namespace Scriptable_Objects.Interaction.IneractionActions
{
    [CreateAssetMenu(fileName = "New InteractionSay", menuName = "ScriptableObjects/InteractionAction/InteractionSaySO", order = 0)]
    public class InteractionSaySO : InteractionActionSO
    {
        [field:SerializeField] public string TextId { get; set; }
        [field:SerializeField] public bool Top { get; set; }
    }
}