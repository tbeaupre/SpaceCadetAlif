using System;

namespace SpaceCadetAlif.Source.Engine.Events
{
    class InputEventArgs : EventArgs
    {
        public Public.Input Input { get; }
        public float Value { get; }

        public InputEventArgs(Public.Input input, float value)
        {
            Input = input;
            Value = value;
        }
    }
}
