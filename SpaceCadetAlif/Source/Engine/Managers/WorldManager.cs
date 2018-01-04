using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Objects;
using System.Collections.Generic;

namespace SpaceCadetAlif.Source.Engine.Managers
{
    static class WorldManager
    {
        public static List<GameObject> ToUpdate { get; private set; }      // The list of active GameObjects.
        public static List<DrawnObject> ToDraw { get; private set; }       // The list of objects to draw.
        private static List<GameObject> toDelete = new List<GameObject>(); // The list of GameObjects to be deleted this game loop.
        public static GameObject FocusObject { get; set; }                 // The GameObject which the camera is focused on.
        public static Room CurrentRoom { get; private set; }               // The Room which is currently occupied.

        public static void Init()
        {
            // Initialize lists.
            ToUpdate = new List<GameObject>();
            ToDraw = new List<DrawnObject>();
        }

        // Called when a portal is triggered.
        public static void ChangeRoom(Room newRoom, Vector2 newPosition)
        {
            if (newRoom != CurrentRoom)
            {
                // Clear out the lists because they will be repopulated for the new room.
                ToUpdate.Clear();
                ToDraw.Clear();
                InputManager.RegisteredActors.Clear();

                CurrentRoom = newRoom;

                FocusObject.OnCreate(); // This is normally called in the constructor to add the object to lists.
            }
            FocusObject.Body.Position = newPosition;
        }

        // Called once per game loop. Updates all GameObjects.
        public static void Update()
        {
            // Update all of the objects.
            foreach (GameObject obj in ToUpdate)
            {
                obj.Update();
            }

            // Delete all the objects that have been marked for deletion.
            if (toDelete.Count != 0)
            {
                foreach (GameObject obj in toDelete)
                {
                    obj.OnDelete(); // Let the object remove itself from relevant lists.
                }
                toDelete.Clear();
            }

            // Update objects' positions and calculate collisions.

            PhysicsManager.Update(CurrentRoom);

            DrawManager.Draw(CurrentRoom, ToDraw);

        }

        // Called when an object wants to delete itself from the world.
        public static void DeleteObject(GameObject obj)
        {
            toDelete.Add(obj);
        }

        // If there is an object being focused, draw everything relative to it.
        public static Vector2 GetFocusOffset()
        {
            if (FocusObject == null)
            {
                return Vector2.Zero;
            }
            return FocusObject.Body.Position;
        }
    }
}
