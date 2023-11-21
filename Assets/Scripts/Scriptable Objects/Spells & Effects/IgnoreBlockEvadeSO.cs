using UnityEngine;

namespace Scriptable_Objects.Spells___Effects
{
    [CreateAssetMenu(fileName = "new IgnoreBlockEvade", menuName = "ScriptableObjects/Effects/IgnoreBlockEvadeSO", order = 0)]
    public class IgnoreBlockEvadeSO : EffectSO
    {
        [field:SerializeField] public bool IgnoreBlock { get; private set; }
        [field:SerializeField] public bool IgnoreEvade { get; private set; }
    }
}