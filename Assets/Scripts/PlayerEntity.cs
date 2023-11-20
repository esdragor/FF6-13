using DG.Tweening;
using UnityEngine;

public class PlayerEntity : Entity
{
    Vector3 newPosition;
    
    private void Start()
    {
        newPosition = transform.position;
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


            // switch (ForwardDirection) // a refacto vite !
            // {
            //     case Direction.Up or D:
            //         newPosition = new Vector3(Mathf.RoundToInt(newPosition.x), Mathf.RoundToInt(position.y + 1), 0);
            //         break;
            //     case Direction.Down:
            //         newPosition = new Vector3(Mathf.RoundToInt(position.x),
            //             Mathf.RoundToInt(position.y - 1), 0);
            //         break;
            //     case Direction.Left:
            //         newPosition = new Vector3(Mathf.RoundToInt(position.x - 1),
            //             Mathf.RoundToInt(position.y), 0);
            //         break;
            //     case Direction.Right:
            //         newPosition = new Vector3(Mathf.RoundToInt(position.x + 1),
            //             Mathf.RoundToInt(position.y), 0);
            //         break;
            //     case Direction.DownLeft:
            //         newPosition = new Vector3(Mathf.RoundToInt(position.x - 1),
            //             Mathf.RoundToInt(position.y - 1), 0);
            //         //transform.DOMove(newPosition, delayToMove);
            //         break;
            //     case Direction.DownRight:
            //         newPosition = new Vector3(Mathf.RoundToInt(position.x + 1),
            //             Mathf.RoundToInt(position.y - 1), 0);
            //         break;
            //     case Direction.UpLeft:
            //         newPosition = new Vector3(Mathf.RoundToInt(position.x - 1),
            //             Mathf.RoundToInt(position.y + 1), 0);
            //         break;
            //     case Direction.UpRight:
            //         newPosition = new Vector3(Mathf.RoundToInt(position.x + 1),
            //             Mathf.RoundToInt(position.y + 1), 0);
            //         break;
            //     case Direction.None:
            //         //transform.DOComplete();
            //         //transform.position = newPosition;
            //         break;
            // }

        if (ForwardDirection != Direction.None)
        {
            transform.DOMove(newPosition, delayToMove);
        }
    }
}