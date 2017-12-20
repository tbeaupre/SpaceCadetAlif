using SpaceCadetAlif.Source.Engine.Input;
using System;

namespace SpaceCadetAlif.Source.Engine.Events
{
    class InputEventArgs : EventArgs
    {
        public Key Key { get; set; }        // The key whose state has changed.
        public KeyState State { get; set; } // The state of the key in question (eg. Pressed, Down, Released).

        public InputEventArgs(Key key, KeyState state)
        {
            Key = key;
            State = state;
        }
    }
}
