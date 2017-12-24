using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceCadetAlif.Source.Engine.Objects
{
    class Prop : DrawnObject
    {
        public bool Interactable { get; } // Determines if a prop can be interacted with.
        public bool Destructible { get; } // Determines if a prop can be destroyed.
        public bool Movable { get; }      // Determines if a prop can be moved.

        public Prop(Sprite sprite, List<Rectangle> collisionBoxes, Vector2 position, bool interact, bool destruct, bool move)
            : base(sprite, collisionBoxes, position)
        {
            Interactable = interact;
            Destructible = destruct;
            Movable = move;
        }
    }
}
