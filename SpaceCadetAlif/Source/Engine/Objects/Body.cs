using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SpaceCadetAlif.Source.Engine.Objects
{
    /*
     * Contains information about a GameObject to be used by the PhysicsManager.
     */
    class Body
    {
        public List<Rectangle> CollisionBoxes { get; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public CollisionType CollisionType { get; set; }


        public Body(List<Rectangle> collisionBoxes, Vector2 position, CollisionType c = CollisionType.GHOST) 
        {
            CollisionBoxes = collisionBoxes;
            Position = position;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
        }
    }
}
