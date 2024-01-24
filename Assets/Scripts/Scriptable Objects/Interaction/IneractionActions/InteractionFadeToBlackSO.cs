using UnityEngine;

namespace Scriptable_Objects.Interaction.IneractionActions
{
    [CreateAssetMenu(fileName = "New InteractionFadeToBlackSO", menuName = "ScriptableObjects/InteractionAction/InteractionFadeToBlackSO", order = 0)]
    public class InteractionFadeToBlackSO : InteractionActionSO
    {
        [field:SerializeField] public float FadeDuration { get; set; }
        [field:SerializeField] public bool FadeIn { get; set; }
    }
}