using System;
using UnityEngine;

namespace Scriptable_Objects.Spells___Effects
{
    [CreateAssetMenu(fileName = "new AllowCommand", menuName = "ScriptableObjects/Effects/AllowCommandSO", order = 0)]
    [Serializable] public class AllowCommandSO : EffectSO
    {
        [field:SerializeField] public AllowedCommands AllowedCommand { get; private set; }
    }
    
    public enum AllowedCommands
    {
        Runic,
        Bushido,
        TwoHandedCompatibility,
    }
}