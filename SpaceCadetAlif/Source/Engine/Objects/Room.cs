
namespace SpaceCadetAlif.Source.Engine.Objects
{
    class Room
    {
        public string BackgroundTag { get; set; }
        public string CollisionTag { get; set; }
        public string ForegroundTag { get; set; }

        public Room(string backgroundTag, string collisionTag, string foregroundTag)
        {
            BackgroundTag = backgroundTag;
            CollisionTag = collisionTag;
            ForegroundTag = foregroundTag;
        }
    }
}
