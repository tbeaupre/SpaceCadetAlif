using System;

namespace SpaceCadetAlif.Source.Engine.Graphics
{
    class DrawSpriteData
    {
        public Sprite Sprite { get; } // The sprite to be drawn.
        public int X { get; }         // The x position to draw at.
        public int Y { get; }         // The y position to draw at.
        public Single Layer { get; }  // The layer to be drawn on.

        public DrawSpriteData(Sprite sprite, int x, int y, Single layer)
        {
            Sprite = sprite;
            X = x;
            Y = y;
            Layer = layer;
        }
    }
}
