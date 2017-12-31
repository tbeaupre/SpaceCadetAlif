using Microsoft.Xna.Framework.Graphics;
using SpaceCadetAlif.Source.Engine.Managers;
using System;

namespace SpaceCadetAlif.Source.Engine.Graphics
{
    class SpriteData
    {
        private Guid mTextureID;        // The ID which is used to retrieve the Sprite's texture.
        public int NumFrames { get; }   // The number of frames in the texture.
        public int FrameWidth { get; }  // The Width in pixels of a frame.
        public int FrameHeight { get; } // The Height in pixels of a frame.
        public int Slowdown { get; }    // The number of game loops between frame changes. Defaults to 0.

        public SpriteData(string texturePath, int numFrames, int slowdown = 0)
        {
            mTextureID = ResourceManager.LoadTexture(texturePath);
            NumFrames = numFrames;

            Texture2D texture = ResourceManager.GetTexture(mTextureID);
            FrameWidth = texture.Width / NumFrames;
            FrameHeight = texture.Height;

            Slowdown = slowdown;
        }

        public Texture2D GetTexture()
        {
            return ResourceManager.GetTexture(mTextureID);
        }
    }
}
