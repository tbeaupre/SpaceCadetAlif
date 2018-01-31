using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Graphics;
using SpaceCadetAlif.Source.Engine.Managers;
using System;

namespace SpaceCadetAlif.Source.Engine
{
    abstract class Sprite
    {
        public SpriteData Data { get; }   // The Texture which this Sprite uses.
        protected int mCurrentX;          // The x position of the frame which will be drawn.
        public int CurrentY { get; set; } // The y position of the frame which will be drawn.
        protected int mFrameTimer;        // Applies a slowdown to the animation speed.

        public Sprite(Guid id, int startX, int startY)
        {
            Data = ResourceManager.GetSpriteData(id);
            mCurrentX = startX;
            CurrentY = startY;
            mFrameTimer = Data.Slowdown;
        }

        // Called once per game loop. Applies the animation slowdown.
        public virtual void Update()
        {
            mFrameTimer--;
        }

        // Calculates the source rectangle for drawing by using the current frame.
        public Rectangle GetSourceRect()
        {
            return new Rectangle(Data.FrameWidth * mCurrentX, Data.FrameHeight * CurrentY, Data.FrameWidth, Data.FrameHeight);
        }

        // Calculates the destination rectangle for drawing by using dimensions of a single frame.
        public Rectangle GetDestRect(int x, int y)
        {
            return new Rectangle(x, y, Data.FrameWidth, Data.FrameHeight);
        }
    }
}
