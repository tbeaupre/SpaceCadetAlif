using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Managers;
using SpaceCadetAlif.Source.Engine.Objects;
using SpaceCadetAlif.Source.Engine.TestObjects;
using SpaceCadetAlif.Source.Game;

namespace SpaceCadetAlif.Source.Public
{
    class YourGame
    {
        public YourGame()
        {
            WorldManager.FocusObject = new Cadet();
            WorldManager.ChangeRoom(new Room("Room/2/Background2", "Room/2/CollisionBox2", "Room/2/Foreground2", 3), new Vector2(390, 330));
        }
    }
}
