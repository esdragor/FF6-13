using DG.Tweening;
using Scriptable_Objects.Unit;
using Units;
using UnityEngine;

public class PlayerEntity : Entity
{
    public PlayerController _playerController { get; private set; }
    
    [SerializeField] private float cellSize = 1f;
    
    private Vector3 newPosition;
    private bool moving = false;
    private static readonly int DirectionProperty = Shader.PropertyToID("_Direction");
    private static readonly int MovingProperty = Shader.PropertyToID("_Moving");

    public void InitPlayer(PlayerController controller)
    {
        newPosition = transform.position;
        PlayerCharactersSO so = SO as PlayerCharactersSO;
        if (so != null)
            unitData = new PlayerCharacterData(so,
                so.GrowthTable, 1);
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
                mat.SetFloat(DirectionProperty, 0f);
                newPosition.y = position.y + cellSize;
                break;
            case Direction.Down or Direction.DownLeft or Direction.DownRight:
                mat.SetFloat(DirectionProperty, 1f);
                newPosition.y =position.y - cellSize;
                break;
            case Direction.Left or Direction.DownLeft or Direction.UpLeft:
                mat.SetFloat(DirectionProperty, 2f);
                newPosition.x = position.x - cellSize;
                break;
            case Direction.Right or Direction.DownRight or Direction.UpRight:
                mat.SetFloat(DirectionProperty, 2f);
                newPosition.x = position.x + cellSize;
                break;
        }
        
        if (ForwardDirection != Direction.None)
        {
            moving = true;
            transform.DOMove(newPosition, delayToMove).OnComplete(()=>moving = false);
        }
        
        mat.SetFloat(MovingProperty, moving ? 1.0f : 0.0f);
    }
    
    public void InitData(UnitData data)
    {
        unitData = data;
    }
}