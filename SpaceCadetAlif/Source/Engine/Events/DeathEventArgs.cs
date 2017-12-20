using System;

namespace SpaceCadetAlif.Source.Engine.Events
{
    class DeathEventArgs : EventArgs
    {
        public object Source { get; set; } // The object which caused the death.

        public DeathEventArgs(object source)
        {
            Source = source;
        }
    }
}
