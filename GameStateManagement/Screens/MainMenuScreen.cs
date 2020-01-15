#region File Description
/*
 * Author: Shon Vivier
 * File Name: MainMenuScreen.cs
 * Project Name: PASS2
 * Creation Date: 4/13/2019
 * Modified Date: 4/14/2019
 * Description: The main menu screen is the first thing displayed to the user
 * when the game starts up. It hooks up individual menu entires to events that
 * move the scenes forward
*/
#endregion

#region Using Statements
using GameStateManagement.Managers;
using Microsoft.Xna.Framework;
using System;
#endregion

namespace GameStateManagement
{
    class MainMenuScreen : MenuScreen
    {
        #region Initialization
        /// <summary>
        /// Constructor fills in the menu contents. The menu title is set to "Main Menu"
        /// </summary>
        public MainMenuScreen() : base("Main Menu")
        {
            // Create our menu entries
            MenuEntry newGameMenuEntry = new MenuEntry("New Game");
            MenuEntry loadGameMenuEntry = new MenuEntry("Load Game");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            // Hook up menu event handlers
            newGameMenuEntry.Selected += NewGameMenuEntrySelected;
            loadGameMenuEntry.Selected += LoadGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu
            MenuEntries.Add(newGameMenuEntry);
            MenuEntries.Add(loadGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }
        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected
        /// </summary>
        void NewGameMenuEntrySelected(object sender, EventArgs e)
        {
            // Load the match info screen
            LoadingScreen.Load(ScreenManager, true, new MatchupInfoScreen());
        }

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected
        /// </summary>
        void LoadGameMenuEntrySelected(object sender, EventArgs e)
        {
            // Load the player information
            SaveAndLoadGame.LoadGame();

            // Continue onto the match info screen
            LoadingScreen.Load(ScreenManager, true, new MatchupInfoScreen());
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected
        /// </summary>
        void OptionsMenuEntrySelected(object sender, EventArgs e)
        {
            // Set the current menu to options
            ScreenManager.AddScreen(new OptionsMenuScreen());
        }


        /// <summary>
        /// Handler for when the user has cancelled the menu
        /// </summary>
        protected virtual void OnCancel()
        {
            // Exit the game
            ScreenManager.Game.Exit();
        }


        /// <summary>
        /// Helper overload makes it easy to use OnCancel as a MenuEntry event handler
        /// </summary>
        protected void OnCancel(object sender, EventArgs e)
        {
            // Exit the game
            OnCancel();
        }

        #endregion
    }
}
