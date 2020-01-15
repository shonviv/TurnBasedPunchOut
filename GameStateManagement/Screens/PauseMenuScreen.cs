#region File Description
/*
 * Author: Shon Vivier
 * File Name: PauseMenuScreen.cs
 * Project Name: GameStateManagement
 * Creation Date: 4/13/2019
 * Modified Date: 4/15/2019
 * Description: This class displays the puase menu. The pause menu comes up over 
 * the top of the game giving the player options to resume or quit and save.
*/
#endregion

#region Using Statements
using GameStateManagement.Managers;
using Microsoft.Xna.Framework;
using System;
#endregion

namespace GameStateManagement
{
    class PauseMenuScreen : MenuScreen
    {
        #region Initialization
        
        /// <summary>
        /// The constructor for the puase menu screen. The class is inherited from the MenuScreen class. The base title screen it takes in is "Paused".
        /// </summary>
        public PauseMenuScreen() : base("Paused")
        {
            // Create our menu entries
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Game");
            MenuEntry quitAndSaveGameMenuEntry = new MenuEntry("Quit and Save");
            
            // Subscribe the menu entries to the event handlers. Resume binds to MenuScreen's default OnCancel
            resumeGameMenuEntry.Selected += OnCancel;
            quitAndSaveGameMenuEntry.Selected += QuitAndSaveGameMenuEntrySelected;

            // Add entries to the menu
            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(quitAndSaveGameMenuEntry);
        }
        #endregion

        #region Handle Input
        
        /// <summary>
        /// Event handler for when the Quit and Save menu entry is selected.
        /// </summary>
        void QuitAndSaveGameMenuEntrySelected(object sender, EventArgs e)
        {
            // Save the game and exit
            SaveAndLoadGame.SaveGame();
            ScreenManager.Game.Exit();
        }
        #endregion
    }
}
