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
        private static List<Actor> toRegister = new List<Actor>();       // List of Actors to add to the Registry once per game loop.
        private static List<Actor> toUnregister = new List<Actor>();     // List of Actors to remove from the Registry once per game loop.
        private static List<Keys> oldState;                              // List of pressed keys from previous frame.
        private static List<Keys> keyState;                              // List of keys currently pressed.

        public static void Init()
        {
            // Initialize lists.
            RegisteredActors = new List<Actor>();
            keyboardBinds = new Dictionary<Input, Keys>();
            gamePadBinds = new Dictionary<Input, Buttons>();
            oldState = new List<Keys>();
            keyState = new List<Keys>();

            // Set default inputs.
            KeyInput = true;
            keyboardBinds.Add(Input.Up, Keys.Up);
            keyboardBinds.Add(Input.Down, Keys.Down);
            keyboardBinds.Add(Input.Left, Keys.Left);
            keyboardBinds.Add(Input.Right, Keys.Right);
            keyboardBinds.Add(Input.Attack, Keys.X);
            keyboardBinds.Add(Input.Jump, Keys.Z);
            keyboardBinds.Add(Input.ChangeWeapons, Keys.C);
        }

        public static void RegisterActor(Actor actor)
        {
            toRegister.Add(actor);
        }

        public static void UnregisterActor(Actor actor)
        {
            toUnregister.Add(actor);
        }

        // Called once per game loop. Checks for input and sends events to registered Actors.
        public static void Update()
        {
            if (toRegister.Count != 0)
            {
                foreach (Actor actor in toRegister)
                {
                    RegisteredActors.Add(actor);
                }
                toRegister.Clear();
            }

            if (toUnregister.Count != 0)
            {
                foreach (Actor actor in toUnregister)
                {
                    RegisteredActors.Remove(actor);
                }
                toUnregister.Clear();
            }

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
                            SendInputEvent(input, 1); // Key is being held down.
                        }
                        else
                        {
                            SendInputEvent(input, 0); // Key was released.
                        }
                    }
                    else
                    {
                        if (keyState.Contains(keyboardBinds[input]))
                        {
                            SendInputEvent(input, 0.5f); // Key is now pressed.
                        }
                    }
                }
            }
        }

        // Creates the event and sends it to all Actors registered for input.
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
