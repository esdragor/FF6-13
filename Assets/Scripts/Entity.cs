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
    [SerializeField] protected UnitSO SO;
    [SerializeField] protected UnitData unitData;

    protected Direction ForwardDirection = Direction.None;
    private UnitSOInstance _unitSoInstance;
    protected Material mat;

    public void Turn(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                _spriteRenderer.flipX = false;
                //_spriteRenderer.flipY = false;
                break;
            case Direction.Down:
                _spriteRenderer.flipX = false;
                //_spriteRenderer.flipY = true;
                break;
            case Direction.Left:
                _spriteRenderer.flipX = false;
                //_spriteRenderer.flipY = false;
                break;
            case Direction.Right:
                _spriteRenderer.flipX = true;
                //_spriteRenderer.flipY = false;
                break;
        }
        
    }
    
    public virtual void Attack()
    {
        Debug.Log("Attack");
    }
    
    public bool TakeDamage(int damage, UnitSO attacker)
    {
        Debug.Log( _unitSoInstance.So.Name + " Take Damage " + damage + " from " + attacker.Name);
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

        mat = new Material(_spriteRenderer.material);
        _spriteRenderer.material = mat;
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