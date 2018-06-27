using SpaceCadetAlif.Source.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCadetAlif.Source.Engine.Physics
{
    struct DirectionPair
    {
        public Direction X { get; set; }
        public Direction Y { get; set; }

        public DirectionPair(Direction A = Direction.DOWN, Direction B = Direction.NONE)
        {
            this.X = A;
            this.Y = B;
        }

    }
}
