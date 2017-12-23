namespace SpaceCadetAlif.Source.Engine.Graphics.Sprites
{
    class LoopingSprite : Sprite
    {
        public LoopingSprite(SpriteData data, int startFrame) : base(data, startFrame)
        {
        }

        public override void Update()
        {
            // Check if the current frame can be changed.
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
