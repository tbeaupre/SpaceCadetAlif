using System;

namespace SpaceCadetAlif.Source.Engine.Graphics.Sprites
{
    class AnimatedSprite : Sprite
    {
        private AnimationData mAnimation;           // A sequence of frame numbers which defines an animation.
        private AnimationData? mQueuedAnimation;    // The next animation to play after an uninterruptible one finishes.
        private int mAnimationIndex;                // The index into the animation array.

        public AnimatedSprite(Guid id, AnimationData startAnimation, int startY = 0)
            : base(id, startAnimation[0], startY)
        {
            mAnimation = startAnimation;
            mAnimationIndex = 0;
        }

        // Changes the animation if it is interruptible.
        public void SetAnimation(AnimationData animation)
        {
            if (animation.Animation != mAnimation.Animation)
            {
                if (!mAnimation.Interruptible)
                {
                    mQueuedAnimation = animation;
                }
                else
                {
                    ChangeAnimation(animation);
                    SetCurrentX();
                }
            }
        }
        
        public override void Update()
        {
            // Check if the current frame can be changed.
            if (mFrameTimer <= 0)
            {
                ++mAnimationIndex;
                // Check if the animation is complete.
                if (mAnimationIndex == mAnimation.Animation.Length)
                {
                    if (mQueuedAnimation.HasValue)
                    {
                        ChangeAnimation(mQueuedAnimation.Value);
                        mQueuedAnimation = null;
                    }
                    else if (mAnimation.Loop)
                    {
                        mAnimationIndex = 0; // Loop back to the start.
                    }
                    else
                    {
                        --mAnimationIndex;
                    }
                }
                mFrameTimer = Data.Slowdown;
                SetCurrentX();
            }
            else
            {
                base.Update();
            }
        }

        private void ChangeAnimation(AnimationData animation)
        {
            mAnimation = animation;
            mAnimationIndex = 0;
            mFrameTimer = Data.Slowdown;
        }

        private void SetCurrentX()
        {
            mCurrentX = mAnimation[mAnimationIndex];
        }
    }
}
