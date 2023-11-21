using System;
using Scriptable_Objects.Unit;
using Units;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public static event Action<Entity> OnEntityDying;
    
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected BoxCollider2D _boxCollider2D;
    [SerializeField] protected float delayToMove = 1f;
    [field:SerializeField] public UnitSO SO { get; private set;}
    [field: SerializeField] public UnitData unitData;

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
    
    public bool TakeDamage(int damage, UnitData attacker)
    {
        Debug.Log( unitData.GetName() + " Take Damage " + damage + " from " + attacker.GetName());
        unitData.TakeDamage(damage);
        if (unitData.CurrentHp <= 0)
        {
            Debug.Log(unitData.GetName() + " is dead");
            OnEntityDying?.Invoke(this);
            Destroy(gameObject, 0.1f);
            return true;
        }
        return false;
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
        _spriteRenderer.sprite = unitData.Sprite;

        mat = new Material(_spriteRenderer.material);
        _spriteRenderer.material = mat;
    }

    public void Init()
    {
        unitData = new UnitData(SO);
        AssignSprite();
    }
    
    public void Init(UnitSO unitSo)
    {
        SO = unitSo;
        Init();
    }
}