using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SpaceCadetAlif.Source.Engine.Objects
{
    class Portal : GameObject
    {
        private Room mDestRoom;
        private Vector2 mDestPos;

        public Portal(Room destRoom, Vector2 destPos, List<Rectangle> collisionBoxes, Vector2 position)
            : base(collisionBoxes, position)
        {
            mDestRoom = destRoom;
            mDestPos = destPos;
        }
    }
}
