using SpaceCadetAlif.Source.Engine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCadetAlif.Source.Engine.Managers
{
    static class WorldManager
    {
        private static List<GameObject> toUpdate = new List<GameObject>();
        private static List<GameObject> toDelete = new List<GameObject>();

        public static void Update()
        {
            // Update all of the objects.
            foreach (GameObject obj in toUpdate)
            {
                obj.Update();
            }

            // Delete all of the objects which have been marked for deletion.
            foreach (GameObject obj in toDelete)
            {
                toUpdate.Remove(obj);
            }
            toDelete.Clear();
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
    }
}
