using System.Collections.Generic;
using UnityEngine;

namespace Scriptable_Objects.Unit
{
    [CreateAssetMenu(fileName = "new PlayerCharactersSO", menuName = "ScriptableObjects/Units/PlayerCharactersSO", order = 0)]
    public class PlayerCharactersSO : UnitSO
    {
        [field:Header("---PlayerCharactersSO---")]
        [field:SerializeField] public int EscapeRate { get; private set; }
    }
}