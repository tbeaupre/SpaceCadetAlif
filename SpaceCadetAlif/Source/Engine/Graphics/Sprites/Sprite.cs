using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Graphics;
using SpaceCadetAlif.Source.Engine.Objects;

namespace SpaceCadetAlif.Source.Engine
{
    abstract class Sprite
    {
        protected GameObject parent;
        public SpriteData Data { get; }
        protected int mCurrentFrame;
        protected int mFrameTimer;

        public Sprite(GameObject parent, SpriteData data, int startFrame)
        {
            Data = data;
            mCurrentFrame = startFrame;
            mFrameTimer = data.Slowdown;
        }

        public virtual void Update()
        {
            mFrameTimer--;
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
