using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine;
using SpaceCadetAlif.Source.Engine.Graphics.Sprites;
using SpaceCadetAlif.Source.Engine.Managers;
using SpaceCadetAlif.Source.Engine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceCadetAlif.Source.Engine.Events;
using SpaceCadetAlif.Source.Engine.Physics;

namespace SpaceCadetAlif.Source.Game
{
    class Bullet : Actor
    {
        public Bullet(Vector2 position, GunsEnum type, bool up) :
            base(
                new List<Sprite> {
                    new ManualSprite(ResourceManager.LoadSpriteData("Actor/Ally/Player/Guns/Projectiles/Projectiles", 5, 2), (int)type, up?1:0)},
                new List<Rectangle> { new Rectangle(0, 1, 6, 3) },
                position, 0)
        {
            CollisionListener += _OnCollision;
            Body.CollisionType = CollisionType.SOFT;

            if (up)
            {
                Body.Velocity = new Vector2(2, -2);
            }
            else
            {
                Body.Velocity = new Vector2(3, 0);
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

            if (collidedWith == null || collidedWith.Body.CollisionType == CollisionType.SOLID)
            {
                WorldManager.DeleteObject(this);
            }
        }
    }
}
