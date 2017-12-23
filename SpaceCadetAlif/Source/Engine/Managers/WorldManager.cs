using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Objects;
using System.Collections.Generic;

namespace SpaceCadetAlif.Source.Engine.Managers
{
    static class WorldManager
    {
        private static List<GameObject> toUpdate = new List<GameObject>(); // The list of active GameObjects.
        private static List<GameObject> toDelete = new List<GameObject>(); // The list of GameObjects to be deleted this game loop.
        public static GameObject FocusObject { get; set; }                 // The GameObject which the camera is focused on.
        public static Room CurrentRoom                                     // The Room which is currently occupied.
        {
            get
            {
                return CurrentRoom;
            }

            set
            {
                CurrentRoom = value;
                PhysicsManager.InitEnvironment(value);
            }
        }

        // Called once per game loop. Updates all GameObjects.
        public static void Update()
        {
            // Update all of the objects.
            foreach (GameObject obj in toUpdate)
            {
                obj.Update();
            }

            // Delete all the objects that have been marked for deletion.
            if (toDelete.Count != 0)
            {
                foreach (GameObject obj in toDelete)
                {
                    toUpdate.Remove(obj);
                }
                toDelete.Clear();
            }

            // Update objects' positions and calculate collisions.
            PhysicsManager.Update(toUpdate);

            DrawManager.Draw(CurrentRoom, toUpdate, GetFocusOffset());
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

        // If there is an object being focused, draw everything relative to it.
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
