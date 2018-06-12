
using System;

namespace SpaceCadetAlif.Source.Engine.Graphics.Sprites
{
    class ManualSprite : Sprite
    {
        public ManualSprite(Guid id, int startX = 0, int startY = 0) : base(id, startX, startY)
        {
        }

        // Changes the current frame immediately.
        public void SetFrame(int x, int y)
        {
            mCurrentX = x;
            CurrentY = y;
        }

        // There is no need for the frame timer to be updated.
        public override void Update()
        {
        }
    }
}
