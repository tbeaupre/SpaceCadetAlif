
using Microsoft.Xna.Framework.Graphics;
using SpaceCadetAlif.Source.Engine.Managers;
using System;

namespace SpaceCadetAlif.Source.Engine.Objects
{
    class Room
    {
        private Guid mBackgroundID;
        private Guid mCollisionID;
        private Guid mForegroundID;

        public Room(string backgroundPath, string collisionPath, string foregroundPath)
        {
            mBackgroundID = ResourceManager.LoadTexture(backgroundPath);
            mCollisionID = ResourceManager.LoadTexture(collisionPath);
            mForegroundID = ResourceManager.LoadTexture(foregroundPath);
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
