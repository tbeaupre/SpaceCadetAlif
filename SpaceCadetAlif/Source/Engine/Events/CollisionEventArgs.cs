using SpaceCadetAlif.Source.Engine.Objects;
using SpaceCadetAlif.Source.Engine.Utilities;
using System;

namespace SpaceCadetAlif.Source.Engine.Events
{
    class CollisionEventArgs : EventArgs
    {
        public GameObject A { get; set; }
        public GameObject B { get; set; }
        public Direction Direction { get; set; }

        public CollisionEventArgs(GameObject a, GameObject b, Direction direction)
        {
            A = a;
            B = b;
            Direction = direction;
        }
    }
}
