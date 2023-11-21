using UnityEngine;

namespace Scriptable_Objects.Items
{
    public abstract class ItemSO : ScriptableObject
    {
        [field:Header("---ItemSO---")]
        [field:SerializeField] public string Name { get; protected set; }
        [field:SerializeField] public string Description { get; protected set; } //TODO: auto generate description from stats?
        [field:SerializeField] public int MaxQuantity { get; protected set; }
        [field:SerializeField] public int Price { get; protected set; }
        [field:SerializeField] public Sprite Sprite { get; protected set; }
    }
}