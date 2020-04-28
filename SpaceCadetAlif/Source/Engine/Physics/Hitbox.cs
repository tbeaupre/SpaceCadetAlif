using Microsoft.Xna.Framework;

namespace SpaceCadetAlif.Source.Engine.Physics
{
    public abstract class CollisionBox
    {
        public abstract CollisionType CollisionType { get; set; }
        public abstract bool Intersects(CollisionBox other);
    };

    public class RectangleCollisionBox : CollisionBox
    {
        public Rectangle Box { get; set; }
        public override CollisionType CollisionType { get; set; }

        public override bool Intersects(CollisionBox other)
        {
            if (object.ReferenceEquals(other.GetType(), this.GetType()))
            {
                return ((RectangleCollisionBox)other).Box.Intersects(Box);
            }

        }
    }

}