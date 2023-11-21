using System;
using Scriptable_Objects.Unit;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected BoxCollider2D _boxCollider2D;
    [SerializeField] protected float delayToMove = 1f;
    [SerializeField] protected UnitSO SO;

    protected Direction ForwardDirection = Direction.None;
    private UnitSOInstance _unitSoInstance;

    public void Turn(Direction dir)
    {
        Debug.Log("Turned " + dir);

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
    
    public virtual void TakeDamage(int damage, UnitSO attacker)
    {
        Debug.Log( _unitSoInstance.So.Name + " Take Damage " + damage + " from " + attacker.Name);
        _unitSoInstance.currentHp -= damage;
        if (_unitSoInstance.currentHp <= 0)
        {
            Debug.Log(_unitSoInstance.So.Name + " is dead");
            Destroy(gameObject);
        }
    }
    
    public void Move(Direction dir)
    {
        ForwardDirection = dir;
        Debug.Log("Moved " + dir);
    }
    
    public void AssignSprite()
    {
        _spriteRenderer.sprite = SO.Sprite;
    }

    public void Init()
    {
        _unitSoInstance = SO.CreateInstance();
        AssignSprite();
    }
    
    public void Init(UnitSO unitSo)
    {
        SO = unitSo;
        Init();
    }
}