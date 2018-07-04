
namespace SpaceCadetAlif.Source.Engine.Graphics.Sprites
{
    /// <summary>
    /// Struct of all data relevant to an animation, including looping and interruption.
    /// </summary>
    struct AnimationData
    {
        public int[] Animation { get; }
        public bool Interruptible { get; }
        public bool Loop { get; }

        public AnimationData(int[] animation, bool interruptible, bool loop)
        {
            Animation = animation;
            Interruptible = interruptible;
            Loop = loop;
        }

        // Define the indexer to allow client code to use [] notation.
        public int this[int i]
        {
            get { return Animation[i]; }
        }
    }
}
