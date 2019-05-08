using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DnDGame
{
    public enum MouseButtons { LeftButton, RightButton }

    public class KeyboardManager
    {

        public KeyboardState CurrentKeyboardState = new KeyboardState();

        public KeyboardState PrevKeyboardState = new KeyboardState();

        public void Update()
        {
            PrevKeyboardState = CurrentKeyboardState; 
            CurrentKeyboardState = Keyboard.GetState(); 

        }

        //Check if a key was pressed on this update
        public bool IsKeyPress(Keys key)
        {
            return (CurrentKeyboardState.IsKeyDown(key) &&
                PrevKeyboardState.IsKeyUp(key));
        }

		//Check if a key was being held down on this update.
        public bool IsKeyDown(Keys key)
        { return (CurrentKeyboardState.IsKeyDown(key)); }

		//Check if a key was released on this update.
        public bool IsKeyRelease(Keys key)
        {
            return (PrevKeyboardState.IsKeyDown(key) &&
                CurrentKeyboardState.IsKeyUp(key));
        }


    }
}