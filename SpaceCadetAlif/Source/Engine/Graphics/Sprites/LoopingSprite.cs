
using System;

namespace SpaceCadetAlif.Source.Engine.Graphics.Sprites
{
    class LoopingSprite : Sprite
    {
        public LoopingSprite(Guid id, int startX = 0, int startY = 0) : base(id, startX, startY)
        {
        }

        public override void Update()
        {
            // Check if the current frame can be changed.
            if (mFrameTimer <= 0)
            {
                mCurrentX = (mCurrentX + 1) % Data.Columns;
                mFrameTimer = Data.Slowdown;
            }
            else
            {
                base.Update();
            }
        }
    }
}
