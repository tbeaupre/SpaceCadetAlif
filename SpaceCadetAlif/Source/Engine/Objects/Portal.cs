using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Events;
using SpaceCadetAlif.Source.Engine.Managers;
using System.Collections.Generic;

namespace SpaceCadetAlif.Source.Engine.Objects
{
    class Portal : GameObject
    {
        private Room mDestRoom;
        private Vector2 mDestPos;

        public Portal(Room destRoom, Vector2 destPos, List<Rectangle> collisionBoxes, Vector2 position)
            : base(collisionBoxes, position,Vector2.Zero)
        {
            mDestRoom = destRoom;
            mDestPos = destPos;
        }

        private void _OnCollision(CollisionEventArgs e)
        {
            if (e.A == WorldManager.FocusObject || e.B == WorldManager.FocusObject)
            {
                WorldManager.ChangeRoom(mDestRoom, mDestPos);
            }
        }
    }
}
