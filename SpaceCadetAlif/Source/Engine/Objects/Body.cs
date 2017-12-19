using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SpaceCadetAlif.Source.Engine.Objects
{
    /*
     * The abstract parent class of both Actors and Props.
     * Allows them to be used by the PhysicsManager in the same way.
     */

    abstract class Body
    {
        public Sprite Sprite { get; }
        public List<Rectangle> CollisionBoxes { get; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }

        protected Body(Sprite sprite, List<Rectangle> collisionBoxes, Vector2 position)
        {
            Sprite = sprite;
            CollisionBoxes = collisionBoxes;
            Position = position;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
        }
    }
}
