using System;
using System.Collections.Generic;
using System.Linq;
using Units;
using UnityEngine;

namespace Scriptable_Objects.Unit
{
    public abstract class UnitSO : ScriptableObject
    {
        [field:Header("---UnitSO---")]
        
        [field:SerializeField] public string Name { get; private set; }
        
        [field:Header("Stats")]
        [field:SerializeField] public int Strength { get; private set; }
        [field:SerializeField] public int Agility { get; private set; }
        [field:SerializeField] public int Stamina { get; private set; }
        [field:SerializeField] public int Magic { get; private set; }
        [field:Space]
        [field:SerializeField] public int Attack { get; private set; }
        [field:Space]
        [field:SerializeField] public int Defence { get; private set; }
        [field:SerializeField] public int MagicDefence { get; private set; }
        [field:SerializeField] public int Evasion { get; private set; }
        [field:SerializeField] public int MagicEvasion { get; private set; }
        
        [field:Space]
        [field:Header("Resistances")]
        [field:SerializeField] public List<ElementResistance> Resistances { get; private set; }
        [field:SerializeField] public List<AlterationImmunity> AlterationImmunity { get; private set; } //Change to ?
        [field:Space]
        [field:Header("Misc")]
        [field:SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public float ArrowCursorHeight { get; private set; } = 0.6f;
        
        [Header("Character Material")]
        [SerializeField] private Material characterMaterial;
        [SerializeField] private Sprite sprite;
        [SerializeField] private Vector2Int spriteSize;
        [SerializeField] private Vector2Int spriteSheetSize;
        [SerializeField] private Vector2Int spriteSheetOffset;
        [SerializeField] private Vector2Int spriteSheetPadding;
        [SerializeField] private bool startWithDown;
        [SerializeField] private bool verticalRead;
        [SerializeField] private int startIndex;
        [SerializeField] private int animationIndexOffset;
        [SerializeField] private int idleFrame;
        [SerializeField] private int framesPerState;
        
        public void ApplyMaterialToEntity(SpriteRenderer renderer,int direction = 0)
        {
            var mat = new Material(characterMaterial);
            
            mat.SetTexture("_MainTex", sprite.texture);
            
            mat.SetVector("_SpriteSize", (Vector2)spriteSize);
            mat.SetVector("_SpriteSheetSize", (Vector2)spriteSheetSize);
            mat.SetVector("_SpriteSheetOffset", (Vector2)spriteSheetOffset);
            mat.SetVector("_SpreadsheetPadding", (Vector2)spriteSheetPadding);
            
            mat.SetFloat("_StartWithDown", startWithDown ? 1.0f : 0.0f);
            mat.SetFloat("_VerticalRead", verticalRead ? 1.0f : 0.0f);
            mat.SetFloat("_StartIndex", startIndex);
            mat.SetFloat("_Animation_Index_Offset", animationIndexOffset);
            mat.SetFloat("_IdleFrame", idleFrame);
            mat.SetFloat("_FramesPerState", framesPerState);
            
            mat.SetFloat("_Direction", direction);
            
            renderer.material = mat;
            renderer.sprite = sprite;
            
            var transform = renderer.transform;
            var scale = transform.localScale;
            if(spriteSheetSize.x != 0) scale.x = spriteSize.x / (float)spriteSheetSize.x;
            if(spriteSheetSize.y != 0) scale.y = spriteSize.y / (float)spriteSheetSize.y;
            transform.localScale = scale;
        }
        
        [ContextMenu("Init All")]
        public void InitAll()
        {
            InitAllResistances();
            InitAllImmunity();
        }
        
        [ContextMenu("Init All Resistances")]
        public void InitAllResistances()
        {
            Resistances = new List<ElementResistance>();
            
            for (int i = 0; i < Enum.GetValues(typeof(Elements)).Length-1; i++)
            {
                Resistances.Add(new ElementResistance((Elements) i, Elements.Heal == (Elements) i ? -100 : 100));
            }
        }

        [ContextMenu("Init All Immunity")]
        public void InitAllImmunity()
        {
            AlterationImmunity = new List<AlterationImmunity>();

            for (int i = 0; i < Enum.GetValues(typeof(Alterations)).Length; i++)
            {
                AlterationImmunity.Add(new AlterationImmunity((Alterations)i, false));
            }
        }
        
        public virtual int GetAttack(UnitData data)
        {
            return 0;
        }
    }
    
    [Serializable] public class ElementResistance
    {
        [field:SerializeField] public string name { get; private set; }
        [field:SerializeField] public Elements Element { get; private set; }
        [field:SerializeField] public int Resistance { get; private set; }
        
        public ElementResistance(Elements element, int resistance)
        {
            Element = element;
            Resistance = resistance;
            name = $"{element}";
        }
    }
    
    [Serializable] public class AlterationImmunity
    {
        [field:SerializeField] public string name { get; private set; }
        [field:SerializeField] public Alterations Alteration { get; private set; }
        [field:SerializeField] public bool Immunity { get; private set; }
        
        public AlterationImmunity(Alterations alterations, bool immunity)
        {
            Alteration = alterations;
            Immunity = immunity;
            name = $"{alterations}";
        }
    }
    


    
    
    // TODO: move theses enums outside of here
    public enum Stats
    {
        Strength,
        Agility,
        Stamina,
        Magic,
        Attack,
        HitRate,
        Defence,
        MagicDefence,
        Evasion,
        MagicEvasion
    }

    public enum Alterations
    {
        Darkness,
        Zombie,
        Poison,
        MagiTek,
        Clear,
        Imp,
        Petrify,
        Fatal,
        Condemned,
        Critical,
        Image,
        Mute,
        Berserk,
        Confuse,
        Sap,
        Psyche,
        Float,
        Regen,
        Slow,
        Haste,
        Stop,
        Shell,
        Safe,
        Reflect,
        Suplex,
        Scan,
        Sketch,
        Control,
        Fractional,
        AllDamage,
        Death
    }
    
    /*
        - -100% = absorption,
        - 0% = annulation,
        - 200% = faiblesse
     */
    public enum Elements
    {
        Water,
        Fire,
        Lightning,
        Ice,
        InstantDeath,
        Poison,
        Holy,
        Heal,
        Earth,
        Wind,
        Physical,
        None
    }
}
