using SpaceCadetAlif.Source.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCadetAlif.Source.Engine.Physics
{
    class DirectionPair
    {
        public Direction A { get; set; }
        public Direction B { get; set; }

        public DirectionPair(Direction A = Direction.NONE, Direction B = Direction.NONE)
        {
            this.A = A;
            this.B = B;
        }

        public void add(Direction d)
        {
            if (A == d || A == Direction.NONE)
            {
                A = d;
            }
            else
            {
                if (B != Direction.NONE)
                {
                    B = d;
                }
            }
        }

        public bool contains(Direction d)
        {
            return A == d || B == d;
        }
    }
}
