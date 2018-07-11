using Newtonsoft.Json;
using SpaceCadetAlif.Source.Engine.Objects;
using System.Collections.Generic;

namespace SpaceCadetAlif.Source.Engine.Map
{
    /// <summary>
    /// Struct for all data relevant to a room. All data is stored in a JSON file.
    /// </summary>
    struct RoomData
    {
        public string Tag { get; }
        public int ParallaxFactor { get; }
        public string BackgroundPath { get; }
        public string CollisionPath { get; }
        public string ForegroundPath { get; }
        public List<Portal> Portals { get; }
        public List<Actor> Actors { get; }
        public List<Prop> Props { get; }

        [JsonConstructor]
        public RoomData(string tag, int plaxFactor, string background, string collision, string foreground,
            Portal[] portals, Actor[] actors, Prop[] props)
        {
            Tag = tag;
            ParallaxFactor = plaxFactor;
            BackgroundPath = background;
            CollisionPath = collision;
            ForegroundPath = foreground;
            Portals = new List<Portal>(portals);
            Actors = new List<Actor>(actors);
            Props = new List<Prop>(props);
        }
    }
}
