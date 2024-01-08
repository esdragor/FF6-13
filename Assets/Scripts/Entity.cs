using System;
using System.Collections.Generic;
using DG.Tweening;
using Scriptable_Objects.Unit;
using Units;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public static event Action<Entity> OnEntityDying;

    public UnitData unitData;
    
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected GameObject selectorObj;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected BoxCollider2D _boxCollider2D;
    [SerializeField] protected float delayToMove = 1f;
    [field: SerializeField] public UnitSO SO { get; protected set; }

    public Direction ForwardDirection { get; protected set; } = Direction.None;

    [SerializeField] protected Material mat;
    protected bool selectTarget;
    protected Color selectTargetColor = Color.red;

    protected static readonly int DirectionProperty = Shader.PropertyToID("_Direction");

    public void Turn(Direction dir)
    {
        ForwardDirection = dir;

        if (dir == Direction.None) return;

        _spriteRenderer.flipX = dir is Direction.Right or Direction.UpRight or Direction.DownRight;

        if (mat == null) ResetMat();
        
        switch (dir)
        {
            case Direction.Up:
                mat.SetFloat(DirectionProperty, 1f);
                break;
            case Direction.Down:
                mat.SetFloat(DirectionProperty, 0f);
                break;
            case Direction.Left or Direction.Right:
                mat.SetFloat(DirectionProperty, 2f);
                break;
        }

        Debug.Log($"changed to {dir}");
    }

    [ContextMenu("ResetMat")]
    public void ResetMat()
    {
        Debug.Log("Reset mat");
        mat = _spriteRenderer.material;
    }
    
    protected abstract void OnDying();

    public abstract void OnTakeDamage();

    public bool TakeDamage(int damage, Elements element, UnitData attacker, bool ignoreDefence = false,
        bool isPourcentDamage = false)
    {
        if (isPourcentDamage) damage = (int)(unitData.MaxHp * damage / 100f);

        /* TODO:
         * -Defence
         * -Elemental Resistance
         * -Evasion
         * -Critical
         * -Miss
         */

        if (element != Elements.None) damage = (int)(damage * (unitData.Resistance(element) / 100f));


        Debug.Log(unitData.GetName() + " Take Damage " + damage + " from " + attacker.GetName());
        unitData.TakeDamage(damage);
        if (element == Elements.Physical)
        {
            if (transform != null)
                transform.DOShakePosition(0.3f, 0.3f, 10, 90f, false, true);
        }
        OnTakeDamage();
        if (unitData.CurrentHp <= 0)
        {
            Debug.Log(unitData.GetName() + " is dead");
            OnEntityDying?.Invoke(this);
            OnDying();
            return true;
        }
        
        

        return true;
    }

    public void RegenMpDamage(int damage, bool isPourcentDamage = false)
    {
        if (isPourcentDamage) damage = (int)(unitData.MaxMp * damage / 100f);

        unitData.RegenMpDamage(damage);
    }

    public void ApplyAlteration(Alterations alterations, bool ignoreImmunity, bool remove)
    {
        /* TODO:
         * -Immunity
         */

        if (remove)
        {
            unitData.RemoveAlteration(alterations, ignoreImmunity);
            return;
        }

        unitData.AddAlteration(alterations, ignoreImmunity);
    }

    // public void Move()
    // {
    //     Move(ForwardDirection);
    // }
    //
    // public void Move(Direction dir)
    // {
    //     ForwardDirection = dir;
    // }

    public void AssignSprite()
    {
        ShowSelector(false);

        if (_spriteRenderer == null) return;
        _spriteRenderer.sprite = unitData.Sprite;

        if (mat != null) return;
        
        Debug.Log("Changing mat");
        mat = new Material(_spriteRenderer.material)
        {
            name = $"{unitData.GetName()} Entity Material"
        };
        _spriteRenderer.material = mat;
    }

    [ContextMenu("Init monster")]
    public void InitMonster()
    {
        Init(true);
    }
    
    public void Init(bool isMonster)
    {
        unitData = (isMonster) ? new MonsterData(SO) : new PlayerCharacterData(SO as PlayerCharactersSO, 1);
        AssignSprite();
    }

    public void Init(UnitSO unitSo, bool isMonster = false)
    {
        SO = unitSo;
        Init(isMonster);
    }

    public void InitExplorer(UnitSO unitSo, bool isMonster = false)
    {
        SO = unitSo;
        unitData = (isMonster) ? new MonsterData(SO) : new PlayerCharacterData(SO as PlayerCharactersSO, 1);

        mat = SO.ApplyMaterialToEntity(_spriteRenderer);
    }

    public void SelectTarget()
    {
        _spriteRenderer.color = selectTargetColor;
        selectTarget = true;
    }

    public void DeselectTarget()
    {
        _spriteRenderer.color = Color.white;
        selectTarget = false;
    }

    public void ShowSelector(bool value)
    {
        if (selectorObj == null) return;

        selectorObj.transform.localPosition = new Vector3(0, SO.ArrowCursorHeight, 0);
        selectorObj.SetActive(value);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}