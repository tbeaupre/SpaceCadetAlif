using SpaceCadetAlif.Source.Engine.Objects;
using SpaceCadetAlif.Source.Engine.Physics;
using System;

namespace SpaceCadetAlif.Source.Engine.Events
{
    class CollisionEventArgs : EventArgs
    {
        public GameObject A { get; set; }
        public GameObject B { get; set; }
        public Room Room { get; set; }

        public CollisionEventArgs(GameObject a, GameObject b)
        {
            A = a;
            B = b;
        }

        public CollisionEventArgs(GameObject a, Room room)
        {
            A = a;
            Room = room;
        }
    }
}
