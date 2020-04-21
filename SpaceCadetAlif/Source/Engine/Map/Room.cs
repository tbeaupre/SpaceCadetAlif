using Microsoft.Xna.Framework;
using System.Collections.Generic;

public class Room
{
    public int Up { get; set; }
    public int Right { get; set; }
    public int Down { get; set; }
    public int Left { get; set; }

    public bool HasBeenVisited()
    {
        return Up + Right + Down + Left > 0;
    }

    public void MarkVisited(Direction direction)
    {
        switch(direction)
        {
            case Direction.Up:
                Down = 1;
                break;
            case Direction.Right:
                Left = 1;
                break;
            case Direction.Down:
                Up = 1;
                break;
            case Direction.Left:
                Right = 1;
                break;
        }
    }
}
