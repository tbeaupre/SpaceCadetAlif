using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine;
using SpaceCadetAlif.Source.Engine.Graphics.Sprites;
using SpaceCadetAlif.Source.Engine.Managers;
using SpaceCadetAlif.Source.Engine.Objects;
using System.Collections.Generic;
using SpaceCadetAlif.Source.Engine.Events;
using SpaceCadetAlif.Source.Engine.Physics;

namespace SpaceCadetAlif.Source.Game
{
    /// <summary>
    /// This class is for the physical implementation of a bullet - not the data representing its characteristics.
    /// Handles creating the bullet in the correct position, setting its velocity, and handling collisions.
    /// </summary>
    class Bullet : Actor
    {
        private static readonly Vector2 armOffset = new Vector2(4, 11);             // Offset from top left corner of guns sprite to base of arm.
        private static readonly Vector2 bulletOffset = new Vector2(0, -2);          // Offset from top left corner of projectiles sprite to origin of straight bullets.
        private static readonly Vector2 angledBulletOffset = new Vector2(0, -4);    // Offset from top left corner to origin of angled bullets.

        private BulletData mBulletData; // The bullet data of this bullet. Needed for path, onHit, and damage.

        public Bullet(Vector2 position, GunData gunData, bool up, bool mirrored) :
            base(
                new List<Sprite> {
                    new ManualSprite(ResourceManager.LoadSpriteData("Actor/Ally/Player/Guns/Projectiles/Projectiles", 5, 2),
                        (int)gunData.GunType,
                        up ? 1 : 0)},
                new List<Rectangle> { new Rectangle(0, 1, 6, 3) },
                position + CalculateOffset(gunData, up, mirrored),
                0)
        {
            CollisionListener += _OnCollision;
            Body.CollisionType = CollisionType.SOFT;

            mBulletData = gunData.BulletData;

            if (up)
            {
                Body.Velocity = new Vector2((mirrored ? -1 : 1) * mBulletData.AngledSpeed, -mBulletData.AngledSpeed);
            }
            else
            {
                Body.Velocity = new Vector2((mirrored ? -1 : 1) * mBulletData.Speed, 0);
            }

            Mirrored = mirrored;
        }

        // Calculates the position at which the bullet should spawn based on gun data.
        private static Vector2 CalculateOffset(GunData gunData, bool up, bool mirrored)
        {
            Vector2 offset = armOffset + (up ? (gunData.BarrelAngledPos + angledBulletOffset) : (gunData.BarrelPos + bulletOffset));
            if (mirrored)
            {
                offset.X = offset.X * -1;
            }
            return offset;
        }

        private void _OnCollision(CollisionEventArgs e)
        {
            GameObject collidedWith;
            if (e.A == this)
            {
                collidedWith = e.B;
            }
            else
            {
                collidedWith = e.A;
            }

            if (collidedWith == null || collidedWith.Body.CollisionType == CollisionType.SOLID)
            {
                WorldManager.DeleteObject(this);
            }
        }
    }
}
