
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Managers;
using System;

namespace SpaceCadetAlif.Source.Engine.Objects
{
    class Room
    {
        public int ParallaxFactor { get; } // Determines the difference in scroll speed between background and foreground textures. (ex. ParallaxFactor = 2 means the background scrolls half as fast.)
        public Color[] ColorData { get; }
        private Guid mBackgroundID;
        private Guid mCollisionID;
        private Guid mForegroundID;

        public Room(string backgroundPath, string collisionPath, string foregroundPath, int parallaxFactor)
        {
            ParallaxFactor = parallaxFactor;
            mBackgroundID = ResourceManager.LoadTexture(backgroundPath);
            mCollisionID = ResourceManager.LoadTexture(collisionPath);
            mForegroundID = ResourceManager.LoadTexture(foregroundPath);

            ColorData = new Color[GetCollision().Width * GetCollision().Height];
            GetCollision().GetData(ColorData);
        }

        private Texture2D GetTexture(Guid guid)
        {
            return ResourceManager.GetTexture(guid);
        }

        public Texture2D GetBackground()
        {
            return GetTexture(mBackgroundID);
        }

        public Texture2D GetCollision()
        {
            return GetTexture(mCollisionID);
        }

        public Texture2D GetForeground()
        {
            return GetTexture(mForegroundID);
        }
    }
}
