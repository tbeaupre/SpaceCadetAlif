using System;

namespace SpaceCadetAlif.Source.Engine.Graphics
{
    class DrawTextureData
    {
        public string Tag { get; }   // The tag of the resource to be drawn.
        public int X { get; }        // The x position to draw at.
        public int Y { get; }        // The y position to draw at.
        public Single Layer { get; } // The layer to be drawn on.

        public DrawTextureData(string tag, int x, int y, Single layer)
        {
            Tag = tag;
            X = x;
            Y = y;
            Layer = layer;
        }
    }
}
