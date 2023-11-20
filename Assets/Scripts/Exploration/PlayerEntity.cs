using DG.Tweening;
using Scriptable_Objects.Unit;
using UnityEngine;

public class PlayerEntity : Entity
{
    private Vector3 newPosition;
    private PlayerCharacterInfoInstance _playerCharacterInfoInstance;
    
    public void Attack(Entity target)
    {
        Debug.Log("Attack");
        target.TakeDamage(_playerCharacterInfoInstance.So.Attack, _playerCharacterInfoInstance.So);
    }
    
    private void Start()
    {
        newPosition = transform.position;
        _playerCharacterInfoInstance = (SO as PlayerCharactersSO)?.CreateInstance(1);
    }

    private void LateUpdate()
    {
        var position = transform.position;

        switch (ForwardDirection)
        {
            case Direction.Up or Direction.UpLeft or Direction.UpRight:
                newPosition.y = Mathf.RoundToInt(position.y + 1);
                break;
            case Direction.Down or Direction.DownLeft or Direction.DownRight:
                newPosition.y = Mathf.RoundToInt(position.y - 1);
                break;
            case Direction.Left or Direction.DownLeft or Direction.UpLeft:
                newPosition.x = Mathf.RoundToInt(position.x - 1);
                break;
            case Direction.Right or Direction.DownRight or Direction.UpRight:
                newPosition.x = Mathf.RoundToInt(position.x + 1);
                break;
        }
        if (ForwardDirection != Direction.None)
        {
            transform.DOMove(newPosition, delayToMove);
        }
    }
}