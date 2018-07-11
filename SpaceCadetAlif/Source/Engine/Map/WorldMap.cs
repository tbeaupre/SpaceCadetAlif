using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCadetAlif.Source.Engine.Map
{
    class WorldMap
    {
        private Dictionary<string, RoomData> mWorldMap = new Dictionary<string, RoomData>();    // Loaded from a JSON file. Contains every room.
        
        public WorldMap()
        {
            Stream stream = new MemoryStream(Properties.Resources.map);
            using (StreamReader r = new StreamReader(stream))
            {
                string json = r.ReadToEnd();
                List<RoomData> roomList = JsonConvert.DeserializeObject<List<RoomData>>(json);
                foreach (RoomData room in roomList)
                {
                    mWorldMap.Add(room.Tag, room);
                }
            }
        }
    }
}
