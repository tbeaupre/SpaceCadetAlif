using System.Collections.Generic;

namespace SpaceCadetAlif.Source.Engine.Physics
{
    public abstract class Collidable
    {
        public abstract IEnumerable<CollisionBox> CollisionBoxes { get; set; }
    }
}
