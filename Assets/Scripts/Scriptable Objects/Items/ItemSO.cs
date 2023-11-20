using UnityEngine;

namespace Scriptable_Objects.Items
{
    public abstract class ItemSO : ScriptableObject
    {
        [field:Header("---ItemSO---")]
        [field:SerializeField] public string Name { get; private set; }
        [field:SerializeField] public int MaxQuantity { get; private set; }
        [field:SerializeField] public int Price { get; private set; }
        [field:SerializeField] public Sprite Sprite { get; private set; }
    }
}