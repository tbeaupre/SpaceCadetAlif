using System;

namespace SpaceCadetAlif.Source.Engine.Events
{
    class InputEventArgs : EventArgs
    {
        Public.Input Input { get; }
        float Value { get; }

        public InputEventArgs(Public.Input input, float value)
        {
            Input = input;
            Value = value;
        }
    }
}
