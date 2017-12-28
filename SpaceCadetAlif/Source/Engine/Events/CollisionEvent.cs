using SpaceCadetAlif.Source.Engine.Objects;
using SpaceCadetAlif.Source.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCadetAlif.Source.Engine.Events
{
    
    class CollisionEvent : Event
    {

        public GameObject A { get; }
        public GameObject B { get; }
        public Direction direction { get; }


        public CollisionEvent(GameObject a, GameObject b, Direction d)
        {
            A = a;
            B = b;
            direction = d;
        }
    }
}
