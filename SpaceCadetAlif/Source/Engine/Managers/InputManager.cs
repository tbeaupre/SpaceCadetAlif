using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using SpaceCadetAlif.Source.Public;
using SpaceCadetAlif.Source.Engine.Objects;
using SpaceCadetAlif.Source.Engine.Events;

namespace SpaceCadetAlif.Source.Engine.Managers
{
    static class InputManager
    {
        private static Dictionary<Input, Keys> keyboardBinds;            // The current Keyboard bindings.
        private static Dictionary<Input, Buttons> gamePadBinds;          // The current GamePad bindings.
        public static bool KeyInput { get; set; }                        // True for Keyboard, False for GamePad.
        public static List<Actor> RegisteredActors { get; private set; } // List of Actors to send events to.
        private static List<Keys> oldState;                              // List of pressed keys from previous frame.
        private static List<Keys> keyState;                              // List of keys currently pressed.

        public static void Init()
        {
            // Initialize lists.
            RegisteredActors = new List<Actor>();
            keyboardBinds = new Dictionary<Input, Keys>();
            gamePadBinds = new Dictionary<Input, Buttons>();
            oldState = new List<Keys>();

            // Set default inputs.
            KeyInput = true;
            keyboardBinds.Add(Input.Up, Keys.Up);
            keyboardBinds.Add(Input.Down, Keys.Down);
            keyboardBinds.Add(Input.Left, Keys.Left);
            keyboardBinds.Add(Input.Right, Keys.Right);
            keyboardBinds.Add(Input.Attack, Keys.X);
            keyboardBinds.Add(Input.Jump, Keys.Z);
        }

        public static void Update()
        {
            if (KeyInput)
            {
                // Set the old key State.
                oldState.Clear();
                oldState.AddRange(keyState);

                // Set the new key state.
                keyState = new List<Keys>(Keyboard.GetState().GetPressedKeys());
                
                foreach (Input input in keyboardBinds.Keys)
                {
                    if (oldState.Contains(keyboardBinds[input]))
                    {
                        if (keyState.Contains(keyboardBinds[input]))
                        {
                            SendInputEvent(input, 1);
                        }
                        else
                        {
                            SendInputEvent(input, 0);
                        }
                    }
                    else
                    {
                        if (keyState.Contains(keyboardBinds[input]))
                        {
                            SendInputEvent(input, 0.5f);
                        }
                    }
                }
            }
        }

        private static void SendInputEvent(Input input, float value)
        {
            InputEventArgs inputEventArgs = new InputEventArgs(input, value);
            foreach(Actor actor in RegisteredActors)
            {
                actor.OnInput(inputEventArgs);
            }
        }

    }
}
