using Microsoft.Xna.Framework;

namespace SpaceCadetAlif.Source.Engine.Objects
{
    abstract class Body
    {
        public Sprite Sprite { get; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }

        protected Body(Sprite sprite, Vector2 position)
        {
            Sprite = sprite;
            Position = position;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
        }
    }
}
