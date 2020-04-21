using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

public class RoomGenerator
{
    int maxLength;
    int maxTunnels;
    Room[,] rooms;

    public RoomGenerator(int dimension, int maxLength, int maxTunnels)
    {
        this.maxLength = maxLength;
        this.maxTunnels = maxTunnels;
        rooms = new Room[dimension, dimension];

        // Pick a random starting point.
        Random rand = new Random();
        int currentX = rand.Next(dimension);
        int currentY = rand.Next(dimension);

        int numRemainingTunnels = maxTunnels;

        int currentTunnelLength;
        Direction currentDirection = Direction.Left;
        int randomLength;

        while (numRemainingTunnels > 0)
        {
            // Find a new direction to move in.
            currentDirection = GetRandomDirection(rand, currentDirection);

            // Reset tunnel length and determine next tunnel length.
            currentTunnelLength = 0;
            randomLength = rand.Next(maxLength);

            while (currentTunnelLength < randomLength)
            {
                if (
                    currentX == 0 && currentDirection == Direction.Left ||
                    currentX == dimension - 1 && currentDirection == Direction.Right ||
                    currentY == 0 && currentDirection == Direction.Up ||
                    currentY == dimension - 1 && currentDirection == Direction.Down
                )
                {
                    break;
                }
                else
                {
                    rooms[currentX, currentY].MarkVisited(currentDirection);
                    UpdateCurrentPosition(ref currentX, ref currentY, currentDirection);
                    currentTunnelLength++;
                }
            }

            if (currentTunnelLength > 0)
            {
                numRemainingTunnels--;
            }
        }
    }

    private void UpdateCurrentPosition(ref int x, ref int y, Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                y--;
                break;
            case Direction.Right:
                x++;
                break;
            case Direction.Down:
                y++;
                break;
            case Direction.Left:
                x--;
                break;
        }
    }

    private Direction GetRandomDirection(Random rand, Direction lastDirection)
    {
        Direction randomDirection;
        do
        {
            randomDirection = (Direction)rand.Next(4);
        }
        while (
            randomDirection == lastDirection ||
            randomDirection == OppositeDirection(lastDirection)
        );

        return randomDirection;
    }

    private Direction OppositeDirection(Direction direction)
    {
        switch(direction)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Right:
                return Direction.Left;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            default:
                return Direction.Right;
        }
    }
}
