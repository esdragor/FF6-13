using System;
using System.Collections.Generic;
using Scriptable_Objects.Encounters;
using Scriptable_Objects.Interaction.IneractionActions;
using UnityEngine;

namespace Scriptable_Objects.Interaction
{
    [CreateAssetMenu(fileName = "New Interaction", menuName = "ScriptableObjects/InteractionAction/Interaction", order = 0)]
    public class InteractionSO : ScriptableObject
    {
        [field: SerializeField] public List<InteractionAction> InteractionActions;
    }

    [Serializable] public class InteractionAction
    {
        [field: SerializeField] public float timeBefore = 0f;
        [field: SerializeField] public InteractionActionSO interaction;
        [field: SerializeField] public float timeAfter = 0f;
    }
    
}