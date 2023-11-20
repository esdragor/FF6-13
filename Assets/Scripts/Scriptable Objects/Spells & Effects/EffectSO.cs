using System;
using Scriptable_Objects.Unit;
using UnityEngine;

namespace Scriptable_Objects.Spells___Effects
{
    public abstract class EffectSO : ScriptableObject
    {}
    
    
    [CreateAssetMenu(fileName = "new Stat Change", menuName = "ScriptableObjects/Effects/StatChangeSO", order = 0)]
    [Serializable] public class StatChangeOS : EffectSO
    {
        [field:SerializeField] public Stats Stat { get; private set; }
        [field:SerializeField] public int Amount { get; private set; }
        [field:SerializeField] public bool FlatValue { get; private set; }
        
        public int GetStatChanged(bool flatValue)
        {
            if (flatValue != FlatValue) return 0;
            
            return Amount;
        }
    }

    
    [CreateAssetMenu(fileName = "new ElementsResistance", menuName = "ScriptableObjects/Effects/ElementsResistanceSO", order = 0)]
    [Serializable] public class ElementsResistanceSO : EffectSO
    {
        [field:SerializeField] public Elements Element { get; private set; }
        [field:SerializeField] public int Amount { get; private set; }
        [field:SerializeField] public bool FlatValue { get; private set; }
    }
    
    
    [CreateAssetMenu(fileName = "new AlterationsImmunity", menuName = "ScriptableObjects/Effects/AlterationsImmunitySO", order = 0)]
    [Serializable] public class AlterationsImmunitySO : EffectSO
    {
        [field:SerializeField] public Alterations Alteration { get; private set; }
        [field:SerializeField] public int Amount { get; private set; }
        [field:SerializeField] public bool FlatValue { get; private set; }
    }
    
    
    [CreateAssetMenu(fileName = "new DamageEffect", menuName = "ScriptableObjects/Effects/DamageEffectSO", order = 0)]
    [Serializable] public class DamageEffectSO : EffectSO
    {
        [field:SerializeField] public Elements Element { get; private set; }
        [field:SerializeField] public int Damage { get; private set; }
        [field:SerializeField] public bool FlatValue { get; private set; }
        [field:SerializeField] public bool SingleTarget { get; private set; }
    }
    
    
    [CreateAssetMenu(fileName = "new IgnoreAlteration", menuName = "ScriptableObjects/Effects/IgnoreAlterationSO", order = 0)]
    [Serializable] public class IgnoreAlterationSO : EffectSO
    {
        [field:SerializeField] public Alterations Alteration { get; private set; }
    }
    
    
    [CreateAssetMenu(fileName = "new AddOrRemoveAlteration", menuName = "ScriptableObjects/Effects/AddOrRemoveAlterationSO", order = 0)]
    [Serializable] public class AddOrRemoveAlterationSO : EffectSO
    {
        [field:SerializeField] public Alterations Alteration { get; private set; }
        [field:SerializeField] public bool Remove { get; private set; }
    }
}