using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace SpaceCadetAlif.Source.Engine.Physics
{
    /*
     * Contains information about a GameObject to be used by the PhysicsManager.
     */
    public class Body
    {
        public Vector2 Position 
            { get; 
            set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public CollisionType CollisionType { get; set; } // Determines how collisions with other objects are handled.
        public float Mass { get; set; }
        public Vector2 Gravity { get; set; }
        public IEnumerable<Rectangle> CollisionBoxes { 
            get 
            {
                foreach(var box in _collisionBoxes)
                {
                    yield return new Rectangle((int) Position.X + box.Location.X, (int) Position.Y + box.Location.Y, box.Width, box.Height);
                }
            } 
        }  // The collision boxes associated with this GameObject
        public float Friction { get; set; }
        private List<Rectangle> _collisionBoxes;  // The collision boxes associated with this GameObject
        public Body(List<Rectangle> collisionBoxes, Vector2 position, Vector2 gravity, CollisionType collisionType = CollisionType.SOLID)
        {
            _collisionBoxes = collisionBoxes;
            Position = position;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
            CollisionType = collisionType;
            Gravity = gravity;
            Mass = 2;
            Friction = 0.2f;
        }

        public void UpdateVelocity()
        {
            Velocity += Acceleration + Gravity;
        }

        public void UpdatePosition()
        {
            Position += Velocity;
        }
    }
}