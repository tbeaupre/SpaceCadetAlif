using SpaceCadetAlif.Source.Engine.Objects;
using System;

namespace SpaceCadetAlif.Source.Engine.Graphics.Sprites
{
    class AnimatedSprite : Sprite
    {
        private DrawnObject mParent; // Used to alert the parent object when the animation ends.
        private int[] mAnimation;    // A sequence of frame numbers which defines an animation.
        private int mAnimationIndex; // The index into the animation array.
        private bool mLoop;          // Determines if the animation should loop when it is complete.
        private bool mInterruptible; // Determines if the animation can be interrupted to start a new one.

        public AnimatedSprite(DrawnObject parent, Guid id, bool loop, bool interruptible, int[] animation, int startY = 0)
            : base(id, animation[0], startY)
        {
            mParent = parent;
            mAnimation = animation;
            mAnimationIndex = 0;
            mLoop = loop;
            mInterruptible = interruptible;
        }

        // Changes the animation if it is interruptible.
        public void SetAnimation(int[] animation, bool loop)
        {
            if (mInterruptible)
            {
                mAnimation = animation;
                mAnimationIndex = 0;
            }
        }
        
        public override void Update()
        {
            // Check if the current frame can be changed.
            if (mFrameTimer <= 0)
            {
                mAnimationIndex++;
                // Check if the animation is complete.
                if (mAnimationIndex == mAnimation.Length)
                {
                    if (mLoop)
                    {
                        mAnimationIndex = 0; // Loop back to the start.
                    }
                    else
                    {
                        mParent.OnAnimationComplete(this, EventArgs.Empty); // Alert the parent that the animation is complete.
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
