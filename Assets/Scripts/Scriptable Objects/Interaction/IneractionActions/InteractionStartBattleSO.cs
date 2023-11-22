using Scriptable_Objects.Encounters;
using UnityEngine;

namespace Scriptable_Objects.Interaction.IneractionActions
{
    [CreateAssetMenu(fileName = "New InteractionStartBattle", menuName = "ScriptableObjects/InteractionAction/InteractionStartBattle", order = 0)]
    public class InteractionStartBattleSO : InteractionActionSO
    {
        [field: SerializeField] public EncounterSO Encounter;
    }
}