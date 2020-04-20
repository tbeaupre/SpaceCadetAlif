using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceCadetAlif.Source.Engine.Physics
{
    /*
     * Contains information about a GameObject to be used by the PhysicsManager.
     */
    public class Body
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public CollisionType CollisionType { get; set; } // Determines how collisions with other objects are handled.
        public float Mass { get; set; } = 2;
        public Vector2 Gravity { get; set; }
        public IEnumerable<Rectangle> CollisionBoxesAbsolute {
            get 
            {
                // returns the absolute position of CollisonBoxesRelative.
                foreach(var box in CollisonBoxesRelative)
                {
                    yield return new Rectangle((int) Position.X + box.Location.X, (int) Position.Y + box.Location.Y, box.Width, box.Height);
                }
            }
        }  // The collision boxes associated with this GameObjects
        public float Friction { get; set; } = 0.2f;
        public List<Rectangle> CollisonBoxesRelative { get; set; }  // The collision boxes associated with this GameObject relative to the objects position

        public int Bottom { get => CollisionBoxesAbsolute.Max(o => o.Bottom); }
        public int Top { get => CollisionBoxesAbsolute.Min(o => o.Top); }
        public int Left { get => CollisionBoxesAbsolute.Min(o => o.Left); }
        public int Right { get => CollisionBoxesAbsolute.Max(o => o.Right); }

        private bool lockedBottom = false;
        private bool lockedTop = false;
        private bool lockedRight = false;
        private bool lockedLeft = false;

        public Body(List<Rectangle> collisionBoxes, Vector2 position, Vector2 gravity, CollisionType collisionType = CollisionType.SOLID)
        {
            CollisonBoxesRelative = collisionBoxes;
            Position = position;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
            CollisionType = collisionType;
            Gravity = gravity;
        }

        public void UpdateVelocity()
        {
            var velocity = Velocity + Acceleration + Gravity;
            if (lockedBottom && velocity.Y > 0)
            {
                velocity.Y = 0;
            }
            Velocity = velocity;
        }

        public void UpdatePosition()
        {
            var velocity = new Vector2(Velocity.X, Velocity.Y);
            if (lockedBottom && velocity.Y > 0)
            {
                velocity.Y = 0;
            }
            Position += velocity;
        }

        public void SnapBottom(Rectangle target)
        {
            var diff = Bottom - target.Top;
            Position -= new Vector2(0, diff);
            lockedBottom = true;
        }

        public void Unsnap()
        {
            lockedBottom = false;
            lockedTop = false;
            lockedRight = false;
            lockedLeft = false;
        }
    }
}