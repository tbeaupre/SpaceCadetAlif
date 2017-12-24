using SpaceCadetAlif.Source.Engine.Objects;
using System;

namespace SpaceCadetAlif.Source.Engine.Events
{
    class CollisionEventArgs : EventArgs
    {

        public GameObject A { get; set; }
        public GameObject B { get; set; }

        public CollisionEventArgs(GameObject a, GameObject b)
        {
            A = a;
            B = b;
        }
    }
}
