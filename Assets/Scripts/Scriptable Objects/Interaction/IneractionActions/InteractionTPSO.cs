using UnityEngine;

namespace Scriptable_Objects.Interaction.IneractionActions
{
    [CreateAssetMenu(fileName = "new InteractionTPSO", menuName = "ScriptableObjects/InteractionAction/InteractionTPSO", order = 0)]
    public class InteractionTPSO : InteractionActionSO
    {
        [field:SerializeField] public string UnitID { get; set; }
        [field:SerializeField] public Vector2 TargetPos { get; set; }
    }
}