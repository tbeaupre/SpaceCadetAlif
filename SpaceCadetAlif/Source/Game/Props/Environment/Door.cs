using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine;
using SpaceCadetAlif.Source.Engine.Objects;
using SpaceCadetAlif.Source.Engine.Graphics.Sprites;
using SpaceCadetAlif.Source.Engine.Managers;

namespace SpaceCadetAlif.Source.Game.Props.Environment
{
    /// <summary>
    /// The physical manifestation of a standard door.
    /// Includes a sprite and a portal which moves the player to another area.
    /// </summary>
    class Door : Prop
    {
        private static readonly List<Rectangle> DOOR_COLLISION_BOXES = new List<Rectangle> { new Rectangle(37, 0, 23, 30) };

        private Portal mPortal; // The portal associated with this door.

        public Door(Vector2 position, Room destRoom, Vector2 destPos)
            : base (
                  new List<Sprite> {
                    new ManualSprite(ResourceManager.LoadSpriteData("Prop/Environment/Door", 1)) },
                  new List<Rectangle> { },
                  position,
                  false,
                  false,
                  false)
        {
            mPortal = new Portal(destRoom, destPos, DOOR_COLLISION_BOXES, position);
        }
    }
}
