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
    class Bullet : Actor
    {
        private static readonly Vector2 armOffset = new Vector2(4, 11);
        private static readonly Vector2 bulletOffset = new Vector2(-4, -2);
        private static readonly Vector2 angledBulletOffset = new Vector2(-3, -1);

        private BulletData mBulletData;

        public Bullet(Vector2 position, GunData gunData, bool up) :
            base(
                new List<Sprite> {
                    new ManualSprite(ResourceManager.LoadSpriteData("Actor/Ally/Player/Guns/Projectiles/Projectiles", 5, 2),
                        (int)gunData.GunType,
                        up ? 1 : 0)},
                new List<Rectangle> { new Rectangle(0, 1, 6, 3) },
                position + armOffset + (up ? gunData.BarrelAngledPos + angledBulletOffset: gunData.BarrelPos + bulletOffset),
                0)
        {
            CollisionListener += _OnCollision;
            Body.CollisionType = CollisionType.SOFT;

            mBulletData = gunData.BulletData;

            if (up)
            {
                Body.Velocity = new Vector2(mBulletData.AngledSpeed, -mBulletData.AngledSpeed);
            }
            else
            {
                Body.Velocity = new Vector2(mBulletData.Speed, 0);
            }

        }

        public void _OnCollision(CollisionEventArgs e)
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

            if (collidedWith is Cadet) return;

            if (collidedWith == null || collidedWith.Body.CollisionType == CollisionType.SOLID)
            {
                WorldManager.DeleteObject(this);
            }
        }
    }
}
