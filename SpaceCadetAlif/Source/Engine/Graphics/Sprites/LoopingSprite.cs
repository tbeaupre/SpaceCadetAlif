using SpaceCadetAlif.Source.Engine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCadetAlif.Source.Engine.Graphics.Sprites
{
    class LoopingSprite : Sprite
    {
        public LoopingSprite(GameObject parent, SpriteData data, int startFrame) : base(parent, data, startFrame)
        {
        }

        public override void Update()
        {
            if (mFrameTimer <= 0)
            {
                mCurrentFrame = (mCurrentFrame + 1) % Data.NumFrames;
                mFrameTimer = Data.Slowdown;
            }
            else
            {
                base.Update();
            }
        }
    }
}
