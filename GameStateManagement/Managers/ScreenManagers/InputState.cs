#region File Description
/*
 * Author: Shon Vivier
 * File Name: InputState.cs
 * Project Name: PASS2
 * Creation Date: 4/14/2019
 * Modified Date: 4/15/2019
 * Description: Helper for reading input from keyboard, gamepad, and touch input. This class
 * tracks both the current and previous state of the input devices, and implements query methods
 * for high level input actions such as "move up through the menu" or "pause the game".
*/
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
#endregion

namespace GameStateManagement
{
    public class InputState
    {
        #region Fields

        // Declare the current and last keyboard states
        public KeyboardState CurrentKeyboardStates;
        public KeyboardState LastKeyboardStates;

        #endregion

        #region Initialization
        /// <summary>
        /// Constructs a new input state.
        /// </summary>
        public InputState()
        {
            // Initialize the keyboard states
            CurrentKeyboardStates = new KeyboardState();
            LastKeyboardStates = new KeyboardState();
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Reads the latest state of the keyboard
        /// </summary>
        public void Update()
        {
            // Update the keyboard states
            LastKeyboardStates = CurrentKeyboardStates;
            CurrentKeyboardStates = Keyboard.GetState();
        }


        /// <summary>
        /// Helper for checking if a key was newly pressed during this update
        /// </summary>
        public bool IsNewKeyPress(Keys key)
        {
            return (CurrentKeyboardStates.IsKeyDown(key) && LastKeyboardStates.IsKeyUp(key));
        }


        /// <summary>
        /// Helper for checking if a button was newly pressed during this update.
        /// </summary>
        public bool IsNewButtonPress(Buttons button)
        {
            // Accept input from any player
            return IsNewButtonPress(button);
        }


        /// <summary>
        /// Checks for a "menu select" input action
        /// </summary>
        public bool IsMenuSelect()
        {
            return IsNewKeyPress(Keys.Z);
        }

        /// <summary>
        /// Checks for a "menu select" input action
        /// </summary>
        public bool IsStartButton()
        {
            return IsNewKeyPress(Keys.Enter);
        }

        /// <summary>
        /// Checks for a "menu cancel" input action
        /// </summary>
        public bool IsMenuCancel()
        {
            return IsNewKeyPress(Keys.Escape);
        }

        /// <summary>
        /// Checks for a "menu down" input action
        /// </summary>
        public bool IsMenuLeft()
        {
            return IsNewKeyPress(Keys.Left);
        }

        /// <summary>
        /// Checks for a "menu down" input action
        /// </summary>
        public bool IsMenuRight()
        {
            return IsNewKeyPress(Keys.Right);
        }

        /// <summary>
        /// Checks for a "menu up" input action
        /// </summary>
        public bool IsMenuUp()
        {
            return IsNewKeyPress(Keys.Up);
        }


        /// <summary>
        /// Checks for a "menu down" input action
        /// </summary>
        public bool IsMenuDown()
        {
            return IsNewKeyPress(Keys.Down);
        }


        /// <summary>
        /// Checks for a "pause the game" input action
        /// </summary>
        public bool IsPauseGame()
        {
            return IsNewKeyPress(Keys.Escape);
        }

        #endregion
    }
}
