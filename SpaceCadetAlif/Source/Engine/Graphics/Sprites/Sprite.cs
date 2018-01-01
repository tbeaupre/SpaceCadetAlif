using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Graphics;

namespace SpaceCadetAlif.Source.Engine
{
    abstract class Sprite
    {
        public SpriteData Data { get; } // The Texture which this Sprite uses.
        protected int mCurrentFrame;    // The frame which will be drawn.
        protected int mFrameTimer;      // Applies a slowdown to the animation speed.

        public Sprite(SpriteData data, int startFrame)
        {
            Data = data;
            mCurrentFrame = startFrame;
            mFrameTimer = data.Slowdown;
        }

        // Called once per game loop. Applies the animation slowdown.
        public virtual void Update()
        {
            mFrameTimer--;
        }

        // Calculates the source rectangle for drawing by using the current frame.
        public Rectangle GetSourceRect()
        {
            return new Rectangle(Data.FrameWidth * mCurrentFrame, 0, Data.FrameWidth, Data.FrameHeight);
        }

        // Calculates the destination rectangle for drawing by using dimensions of a single frame.
        public Rectangle GetDestRect(int x, int y)
        {
            return new Rectangle(x, y, Data.FrameWidth, Data.FrameHeight);
        }
    }
}
