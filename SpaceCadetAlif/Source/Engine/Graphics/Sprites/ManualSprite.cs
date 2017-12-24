﻿
namespace SpaceCadetAlif.Source.Engine.Graphics.Sprites
{
    class ManualSprite : Sprite
    {
        public ManualSprite(SpriteData data, int startFrame) : base(data, startFrame)
        {
        }

        // Changes the current frame immediately.
        public void SetFrame(int frame)
        {
            mCurrentFrame = frame;
        }

        // There is no need for the frame timer to be updated.
        public override void Update()
        {
        }
    }
}