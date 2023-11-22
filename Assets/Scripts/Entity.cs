using System;
using DG.Tweening;
using Scriptable_Objects.Unit;
using Units;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public static event Action<Entity> OnEntityDying;
    public UnitData unitData;
    
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected BoxCollider2D _boxCollider2D;
    [SerializeField] protected float delayToMove = 1f;
    [field:SerializeField] public UnitSO SO { get; protected set;}

    public Direction ForwardDirection { get; protected set; } = Direction.None;

    protected Material mat;
    
    private static readonly int DirectionProperty = Shader.PropertyToID("_Direction");

    public void Turn(Direction dir)
    {
        ForwardDirection = dir;
        
        if (dir == Direction.None) return;
        
        _spriteRenderer.flipX = dir is Direction.Right or Direction.UpRight or Direction.DownRight;
        
        switch (dir)
        {
            case Direction.Up :
                mat.SetFloat(DirectionProperty, 0f);
                break;
            case Direction.Down:
                mat.SetFloat(DirectionProperty, 1f);
                break;
            case Direction.Left or Direction.Right:
                mat.SetFloat(DirectionProperty, 2f);
                break;
        }
    }
    
    public bool TakeDamage(int damage, Elements element, UnitData attacker, bool ignoreDefence = false, bool isPourcentDamage = false)
    {
        if (isPourcentDamage) damage = (int) (unitData.MaxHp * damage / 100f);
        
        /* TODO:
         * -Defence
         * -Elemental Resistance
         * -Evasion
         * -Critical
         * -Miss
         */
        
        if (element != Elements.None) damage = (int) (damage * (unitData.Resistance(element) / 100f));
        
        
        Debug.Log( unitData.GetName() + " Take Damage " + damage + " from " + attacker.GetName());
        unitData.TakeDamage(damage);
        if (element == Elements.Physical)
        {
            transform.DOShakePosition(0.3f, 0.3f, 10, 90f, false, true);
        }
        if (unitData.CurrentHp <= 0)
        {
            Debug.Log(unitData.GetName() + " is dead");
            OnEntityDying?.Invoke(this);
            Destroy(gameObject, 0.5f);
            return true;
        }
        return true;
    }

    public void RegenMpDamage(int damage, bool isPourcentDamage = false)
    {
        if (isPourcentDamage) damage = (int) (unitData.MaxMp * damage / 100f);
        
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

    public void Move()
    {
        Move(ForwardDirection);
    }
    
    public void Move(Direction dir)
    {
        ForwardDirection = dir;
    }
    
    public void AssignSprite()
    {
        if (_spriteRenderer == null) return;
        _spriteRenderer.sprite = unitData.Sprite;

        mat = new Material(_spriteRenderer.material);
        _spriteRenderer.material = mat;
    }

    public void Init(bool isMonster)
    {
        unitData =  (isMonster) ? new MonsterData(SO) : new PlayerCharacterData(SO as PlayerCharactersSO, 1);
        AssignSprite();
    }
    
    public void Init(UnitSO unitSo, bool isMonster = false)
    {
        SO = unitSo;
        Init(isMonster);
    }

    public void SelectEnemy()
    {
        _spriteRenderer.color = Color.red;
    }

    public void DeselectEnemy()
    {
        _spriteRenderer.color = Color.white;
    }
}