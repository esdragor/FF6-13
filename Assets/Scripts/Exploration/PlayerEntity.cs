using DG.Tweening;
using Scriptable_Objects.Unit;
using Units;
using UnityEngine;

public class PlayerEntity : Entity
{
    public PlayerController _playerController { get; private set; }
    
    [SerializeField] private float cellSize = 1f;
    
    protected PlayerCharacterInfoInstance _playerCharacterInfoInstance;
    
    private Vector3 newPosition;
    public void InitPlayer(PlayerController controller)
    {
        newPosition = transform.position;
        _playerCharacterInfoInstance = (SO as PlayerCharactersSO)?.CreateInstance(1);
        if (_playerCharacterInfoInstance != null)
            unitData = new PlayerCharacterData(_playerCharacterInfoInstance.So,
                _playerCharacterInfoInstance.So.GrowthTable, 1);
        _playerController = controller;
    }

    private void LateUpdate()
    {
        var position = transform.position;

        switch (ForwardDirection)
        {
            case Direction.Up or Direction.UpLeft or Direction.UpRight:
                newPosition.y = position.y + cellSize;
                break;
            case Direction.Down or Direction.DownLeft or Direction.DownRight:
                newPosition.y =position.y - cellSize;
                break;
            case Direction.Left or Direction.DownLeft or Direction.UpLeft:
                newPosition.x = position.x - cellSize;
                break;
            case Direction.Right or Direction.DownRight or Direction.UpRight:
                newPosition.x = position.x + cellSize;
                break;
        }
        if (ForwardDirection != Direction.None)
        {
            transform.DOMove(newPosition, delayToMove);
        }
    }
    
    public void InitData(UnitData data)
    {
        unitData = data;
    }
}