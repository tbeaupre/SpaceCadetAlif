using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Graphics;

namespace SpaceCadetAlif.Source.Engine
{
    class Sprite
    {
        public SpriteData Data { get; }
        private int mCurrentFrame;
        private int mFrameTimer;

        public Sprite(SpriteData data)
        {
            Data = data;
            mCurrentFrame = 0;
            mFrameTimer = data.Slowdown;
        }

        public void Update()
        {
            if (mFrameTimer == 0)
            {
                mCurrentFrame = (mCurrentFrame + 1) % Data.NumFrames;
                mFrameTimer = Data.Slowdown;
            }
            else
            {
                mFrameTimer--;
            }
        }

        public Rectangle GetSourceRect()
        {
            return new Rectangle(Data.FrameWidth * mCurrentFrame, 0, Data.FrameWidth, Data.FrameHeight);
        }

        public Rectangle GetDestRect(int x, int y)
        {
            return new Rectangle(x, y, Data.FrameWidth, Data.FrameHeight);
        }
    }
}
