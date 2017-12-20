using SpaceCadetAlif.Source.Engine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCadetAlif.Source.Engine.Graphics.Sprites
{
    class AnimatedSprite : Sprite
    {
        private int[] mAnimation;
        private int mAnimationIndex;
        private bool mLoop;

        public AnimatedSprite(GameObject parent, SpriteData data, int[] animation, bool loop) : base(parent, data, animation[0])
        {
            mAnimation = animation;
            mAnimationIndex = 0;
            mLoop = loop;
        }

        public override void Update()
        {
            if (mFrameTimer <= 0)
            {
                mAnimationIndex++;
                if (mAnimationIndex == mAnimation.Length)
                {
                    if (mLoop)
                    {
                        mAnimationIndex = 0;
                    }
                    else
                    {
                        // Send AnimationComplete event to parent.
                    }
                }
                mFrameTimer = Data.Slowdown;
            }
            else
            {
                base.Update();
            }
        }
    }
}
