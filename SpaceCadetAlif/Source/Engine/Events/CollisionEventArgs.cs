using SpaceCadetAlif.Source.Engine.Objects;
using SpaceCadetAlif.Source.Engine.Physics;
using SpaceCadetAlif.Source.Engine.Utilities;
using System;

namespace SpaceCadetAlif.Source.Engine.Events
{
    class CollisionEventArgs : EventArgs
    {
        public GameObject A { get; set; }
        public GameObject B { get; set; }
        public Room Room { get; set; }
        public DirectionPair CollisionDirection { get; set; }

        public CollisionEventArgs(GameObject a, GameObject b, DirectionPair direction)
        {
            A = a;
            B = b;
            CollisionDirection = direction;
        }

        public CollisionEventArgs(GameObject a, Room room, DirectionPair direction)
        {
            A = a;
            Room = room;
            CollisionDirection = direction;
        }
    }
}
