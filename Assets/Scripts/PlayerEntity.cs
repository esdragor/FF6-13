using DG.Tweening;
using UnityEngine;

public class PlayerEntity : Entity
{
    Vector3 newPosition;

    private void LateUpdate()
    {
        var position = transform.position;
        switch (ForwardDirection) // a refacto vite !
        {
            case Direction.Up:
                newPosition = new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y + 1), 0);
                //transform.DOMoveY(Mathf.RoundToInt(transform.position.y + 1), delayToMove);
                break;
            case Direction.Down:
                newPosition = new Vector3(Mathf.RoundToInt(position.x),
                    Mathf.RoundToInt(position.y - 1), 0);
                //transform.DOMoveY(Mathf.RoundToInt(transform.position.y - 1), delayToMove);
                break;
            case Direction.Left:
                newPosition = new Vector3(Mathf.RoundToInt(position.x - 1),
                    Mathf.RoundToInt(position.y), 0);
                //transform.DOMoveX(Mathf.RoundToInt(transform.position.x - 1), delayToMove);
                break;
            case Direction.Right:
                newPosition = new Vector3(Mathf.RoundToInt(position.x + 1),
                    Mathf.RoundToInt(position.y), 0);
                //transform.DOMoveX(Mathf.RoundToInt(transform.position.x + 1), delayToMove);
                break;
            case Direction.DownLeft:
                newPosition = new Vector3(Mathf.RoundToInt(position.x - 1),
                    Mathf.RoundToInt(position.y - 1), 0);
                //transform.DOMove(newPosition, delayToMove);
                break;
            case Direction.DownRight:
                newPosition = new Vector3(Mathf.RoundToInt(position.x + 1),
                    Mathf.RoundToInt(position.y - 1), 0);
                break;
            case Direction.UpLeft:
                newPosition = new Vector3(Mathf.RoundToInt(position.x - 1),
                    Mathf.RoundToInt(position.y + 1), 0);
                break;
            case Direction.UpRight:
                newPosition = new Vector3(Mathf.RoundToInt(position.x + 1),
                    Mathf.RoundToInt(position.y + 1), 0);
                break;
            case Direction.None:
                //transform.DOComplete();
                //transform.position = newPosition;
                break;
        }

        if (ForwardDirection != Direction.None)
        {
            transform.DOMove(newPosition, delayToMove);
        }
    }
}