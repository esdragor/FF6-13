using DG.Tweening;
using UnityEngine;

public class PlayerEntity : Entity
{
    private void LateUpdate()
    {
        switch (ForwardDirection)
        {
            case Direction.Up:
                transform.DOMoveY(Mathf.RoundToInt(transform.position.y + 1), delayToMove);
                break;
            case Direction.Down:
                transform.DOMoveY(Mathf.RoundToInt(transform.position.y - 1), delayToMove);
                break;
            case Direction.Left:
                transform.DOMoveX(Mathf.RoundToInt(transform.position.x - 1), delayToMove);
                break;
            case Direction.Right:
                transform.DOMoveX(Mathf.RoundToInt(transform.position.x + 1), delayToMove);
                break;
            default:
                break;
            
        }
    }
}
