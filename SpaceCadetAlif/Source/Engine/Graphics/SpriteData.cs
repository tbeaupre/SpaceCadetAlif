using Microsoft.Xna.Framework.Graphics;
using SpaceCadetAlif.Source.Engine.Managers;

namespace SpaceCadetAlif.Source.Engine.Graphics
{
    class SpriteData
    {
        public string TextureTag { get; } // The tag which is used to retrieve the Sprite's texture.
        public int NumFrames { get; }     // The number of frames in the texture.
        public int FrameWidth { get; }    // The Width in pixels of a frame.
        public int FrameHeight { get; }   // The Height in pixels of a frame.
        public int Slowdown { get; }      // The number of game loops between frame changes. Defaults to 0.

        public SpriteData(string textureTag, int numFrames, int slowdown = 0)
        {
            TextureTag = textureTag;
            NumFrames = numFrames;

            Texture2D texture = ResourceManager.GetTexture(textureTag);
            FrameWidth = texture.Width / NumFrames;
            FrameHeight = texture.Height;

            Slowdown = slowdown;
        }
    }
}
