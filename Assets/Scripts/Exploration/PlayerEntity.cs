using DG.Tweening;
using Scriptable_Objects.Unit;
using Units;
using UnityEngine;

public class PlayerEntity : Entity
{
    private Vector3 newPosition;
    private PlayerCharacterInfoInstance _playerCharacterInfoInstance;
    public PlayerController _playerController { get; private set; } 
    [SerializeField] private float cellSize = 1f;
    
    private bool moving = false;
    
    public bool Attack(Entity target)
    {
        Debug.Log("Attack");
        return target.TakeDamage(unitData.Attack, _playerCharacterInfoInstance.So);
    }
    
    public void InitPlayer(PlayerController controller)
    {
        newPosition = transform.position;
        _playerCharacterInfoInstance = (SO as PlayerCharactersSO)?.CreateInstance(1);
        if (_playerCharacterInfoInstance != null)
            unitData = new PlayerCharacterData(_playerCharacterInfoInstance.So,
                _playerCharacterInfoInstance.So.GrowthTable, 1);
        _playerController = controller;

        moving = false;
        
        Init();
    }

    private void LateUpdate()
    {
        var position = transform.position;

        switch (ForwardDirection)
        {
            case Direction.Up or Direction.UpLeft or Direction.UpRight:
                mat.SetFloat("_Direction", 0f);
                newPosition.y = position.y + cellSize;
                break;
            case Direction.Down or Direction.DownLeft or Direction.DownRight:
                mat.SetFloat("_Direction", 1f);
                newPosition.y =position.y - cellSize;
                break;
            case Direction.Left or Direction.DownLeft or Direction.UpLeft:
                mat.SetFloat("_Direction", 2f);
                newPosition.x = position.x - cellSize;
                break;
            case Direction.Right or Direction.DownRight or Direction.UpRight:
                mat.SetFloat("_Direction", 2f);
                newPosition.x = position.x + cellSize;
                break;
        }
        
        if (ForwardDirection != Direction.None)
        {
            moving = true;
            transform.DOMove(newPosition, delayToMove).OnComplete(()=>moving = false);
        }
        
        mat.SetFloat("_Moving", moving ? 1.0f : 0.0f);
    }
}