using System;
using Scriptable_Objects.Unit;
using Units;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected BoxCollider2D _boxCollider2D;
    [SerializeField] protected float delayToMove = 1f;
    [field:SerializeField] public UnitSO SO { get; private set;}
    [field: SerializeField] public UnitData unitData;
    
    protected Direction ForwardDirection = Direction.None;

    private UnitSOInstance _unitSoInstance;

    public void Turn(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                _spriteRenderer.flipX = false;
                _spriteRenderer.flipY = false;
                break;
            case Direction.Down:
                _spriteRenderer.flipX = false;
                _spriteRenderer.flipY = true;
                break;
            case Direction.Left:
                _spriteRenderer.flipX = false;
                _spriteRenderer.flipY = false;
                break;
            case Direction.Right:
                _spriteRenderer.flipX = true;
                _spriteRenderer.flipY = false;
                break;
        }
        
    }
    
    public virtual void Attack()
    {
        Debug.Log("Attack");
    }
    
    public bool TakeDamage(int damage, UnitData attacker)
    {
        Debug.Log( _unitSoInstance.So.Name + " Take Damage " + damage + " from " + attacker.GetName());
        _unitSoInstance.currentHp -= damage;
        if (_unitSoInstance.currentHp <= 0)
        {
            Debug.Log(_unitSoInstance.So.Name + " is dead");
            Destroy(gameObject);
            return true;
        }
        return false;
    }
    
    public void Move(Direction dir)
    {
        ForwardDirection = dir;
    }
    
    public void AssignSprite()
    {
        _spriteRenderer.sprite = SO.Sprite;
    }

    public void Init()
    {
        unitData = new UnitData(SO);
        _unitSoInstance = SO.CreateInstance();
        AssignSprite();
    }
    
    public void Init(UnitSO unitSo)
    {
        SO = unitSo;
        Init();
    }
}