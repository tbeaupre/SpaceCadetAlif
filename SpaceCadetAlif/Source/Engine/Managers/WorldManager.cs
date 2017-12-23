using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Objects;
using System.Collections.Generic;

namespace SpaceCadetAlif.Source.Engine.Managers
{
    static class WorldManager
    {
        private static List<GameObject> toUpdate = new List<GameObject>();
        private static List<GameObject> toDelete = new List<GameObject>();
        private static Room currentRoom;
        public static GameObject FocusObject { get; set; }

        public static void Init(Room startRoom)
        {
            currentRoom = startRoom;
        }

        public static void Update()
        {
            // Update all of the objects.
            foreach (GameObject obj in toUpdate)
            {
                obj.Update();
            }

            // Delete all the objects that have been marked for deletion.
            foreach (GameObject obj in toDelete)
            {
                toUpdate.Remove(obj);
            }
            toDelete.Clear();

            DrawManager.Draw(currentRoom, toUpdate, GetFocusOffset());
        }

        // Called when an object wants to delete itself from the world.
        public static void DeleteObject(GameObject obj)
        {
            toDelete.Add(obj);
        }

        // Called when an object is created in order to start updating it.
        public static void CreateObject(GameObject obj)
        {
            toUpdate.Add(obj);
        }

        private static Vector2 GetFocusOffset()
        {
            if (FocusObject == null)
            {
                return Vector2.Zero;
            }
            return FocusObject.Body.Position;
        }
    }
}
