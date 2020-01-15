#region File Description
/*
 * Author: Shon Vivier
 * File Name: MenuScreen.cs
 * Project Name: GameStateManagement
 * Creation Date: 4/13/2019
 * Modified Date: 4/16/2019
 * Description: Base class for screens that contain a menu of options. The user 
 * can move up and down to select an entry, or cancel to back out of the screen.
*/
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
#endregion

namespace GameStateManagement
{
    abstract class MenuScreen : GameScreen
    {
        #region Fields
        // Create and intialize a list of menu entries
        List<MenuEntry> menuEntries = new List<MenuEntry>();

        // Define the currently selected entry and menu title
        int selectedEntry = 0;
        string menuTitle;

        #endregion

        #region Properties
        /// <summary>
        /// Gets the list of menu entries, so derived classes can add
        /// or change the menu contents
        /// </summary>
        protected IList<MenuEntry> MenuEntries
        {
            get { return menuEntries; }
        }
        
        #endregion

        #region Initialization
        /// <summary>
        /// Constructor for the menu screen that takes in a title to set for the menu
        /// </summary>
        public MenuScreen(string menuTitle)
        {
            // Define the menu title
            this.menuTitle = menuTitle;

            // Set the transition times
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }
        #endregion

        #region Handle Input
        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu
        /// </summary>
        public override void HandleInput(InputState input)
        {
            // Move to the previous menu entry
            if (input.IsMenuUp())
            {
                // Decrement the currently selected entry
                selectedEntry--;

                // If the entry is less than 0, wrap it back around to the last entry
                if (selectedEntry < 0)
                    selectedEntry = menuEntries.Count - 1;
            }

            // Move to the next menu entry
            if (input.IsMenuDown())
            {
                // Increment the currently selected entry
                selectedEntry++;

                // If the entry is the last entry, wrap it back around to first entry
                if (selectedEntry >= menuEntries.Count)
                    selectedEntry = 0;
            }

            // Check to see if a menu was selected
            if (input.IsMenuSelect())
            {
                // Trigger the event for the selected entry
                OnSelectEntry(selectedEntry);
            }

            // Otherwhise, check if the user cancelled the menu
            else if (input.IsMenuCancel())
            {
                // Call the cancel method to take the user out
                OnCancel();
            }
        }


        /// <summary>
        /// Handler for when the user has chosen a menu entry
        /// </summary>
        protected virtual void OnSelectEntry(int entryIndex)
        {
            // Raise the event for the menu entry at the specific index
            menuEntries[entryIndex].OnSelectEntry();
        }


        /// <summary>
        /// Handler for when the user has cancelled the menu
        /// </summary>
        protected virtual void OnCancel()
        {
            // Exit the screen and return to the previous menu if there was one
            ExitScreen();
        }


        /// <summary>
        /// Helper overload makes it easy to use OnCancel as a MenuEntry event handler
        /// </summary>
        protected void OnCancel(object sender, EventArgs e)
        {
            // Cancel and go back to the previous menu
            OnCancel();
        }
        #endregion

        #region Update and Draw
        /// <summary>
        /// Allows the screen to position the menu entries. By default
        /// all menu entries are lined up in a vertical list, centered on the screen
        /// </summary>
        protected virtual void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end)
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Start at Y = 175; each X value is generated per entry
            Vector2 position = new Vector2(0f, 175f);

            // Update each menu entry's location in turn
            for (int i = 0; i < menuEntries.Count; i++)
            {
                // Define the current menu entry we are operating with
                MenuEntry menuEntry = menuEntries[i];
                
                // Centre each entry horizontally
                position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - menuEntry.GetWidth(this) / 2;

                // Animate the menu entries
                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                // Set the entry's position
                menuEntry.Position = position;

                // Move down for the next entry the size of this entry
                position.Y += menuEntry.GetHeight(this);
            }
        }


        /// <summary>
        /// Updates the menu
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update each nested MenuEntry object
            for (int i = 0; i < menuEntries.Count; i++)
            {
                // Get whether or not the menu entry is selected
                bool isSelected = IsActive && (i == selectedEntry);

                // Update the menu entry with the selected bool
                menuEntries[i].Update(this, isSelected, gameTime);
            }
        }


        /// <summary>
        /// Draws the menu
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Aligns the menu entry locations before drawing
            UpdateMenuEntryLocations();

            // Define the graphics and sprite batch
            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            // Begin drawing
            spriteBatch.Begin();

            // Draw each menu entry in turn
            for (int i = 0; i < menuEntries.Count; i++)
            {
                // Store the current menu entry
                MenuEntry menuEntry = menuEntries[i];

                // Check to see if it is selected
                bool isSelected = IsActive && (i == selectedEntry);

                // Draw the menu
                menuEntry.Draw(this, isSelected, gameTime);
            }

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end)
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 90);
            Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;

            // Define the colour based on the state of the animation we are in
            Color titleColor = new Color(255, 255, 255) * TransitionAlpha;

            // Set a title scale
            float titleScale = 1.25f;

            // Set the title position to the transition offset for the animation
            titlePosition.Y -= transitionOffset * 100;

            // Draw the menu title at the top
            spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0, titleOrigin, titleScale, SpriteEffects.None, 0);
            spriteBatch.End();
        }
        
        #endregion
    }
}
