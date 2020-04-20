using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

public class RoomGenerator
{
    int maxLength;
    int maxTurns;
    Room[,] rooms;

    public RoomGenerator(int dimension, int maxLength, int maxTurns)
    {
        this.maxLength = maxLength;
        this.maxTurns = maxTurns;
        rooms = new Room[dimension, dimension];

        // Pick a random starting point.
        Random rand = new Random();
        int currentColumn = rand.Next(dimension);
        int currentRow = rand.Next(dimension);
    }
}
