#region File Description
/*
 * Author: Shon Vivier
 * File Name: OptionsMenuScreen.cs
 * Project Name: GameStateManagement
 * Creation Date: 4/15/2019
 * Modified Date: 4/16/2019
 * Description: The options screen is brought up over the top of the main menu screen, 
 * and gives the user a chance to configure the game and manipulate settings.
*/
#endregion

#region Using Statements
using GameStateManagement.Managers;
using Microsoft.Xna.Framework;
using System;
#endregion

namespace GameStateManagement
{
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        // Create our menu entries
        MenuEntry wipeSaveMenuEntry;
        MenuEntry languageMenuEntry;
        
        // Define the languages that are accessible and the current language
        static string[] languages = { "UK English", "British English", "Australian English", "Canadian English" };
        static int currentLanguage = 0;
        
        #endregion

        #region Initialization
        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen() : base("Options")
        {
            // Create our menu entries (with no text in them by default)
            wipeSaveMenuEntry = new MenuEntry(string.Empty);
            languageMenuEntry = new MenuEntry(string.Empty);

            // Fill in the values for the menu entries
            SetMenuEntryText();

            // Create a back menu entry to take us back to the main screen
            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers
            wipeSaveMenuEntry.Selected += WipeSaveMenuEntrySelected;
            languageMenuEntry.Selected += LanguageMenuEntrySelected;

            // Set our back menu entry to return the screen back to the main menu
            back.Selected += OnCancel;
            
            // Add entries to the menu
            MenuEntries.Add(wipeSaveMenuEntry);
            MenuEntries.Add(languageMenuEntry);
            MenuEntries.Add(back);
        }

        /// <summary>
        /// Fills in the latest values for the options screen menu text
        /// </summary>
        void SetMenuEntryText()
        {
            // Set the wipe save menu entry text and language menu entry text
            wipeSaveMenuEntry.Text = "Wipe Save";
            languageMenuEntry.Text = languages[currentLanguage];
        }

        #endregion

        #region Handle Input
        /// <summary>
        /// Event handler for when the wipe save menu entry is selected
        /// </summary>
        void WipeSaveMenuEntrySelected(object sender, EventArgs e)
        {
            // Wipe the save data
            SaveAndLoadGame.WipeSave();
        }

        /// <summary>
        /// Event handler for when the language menu entry is selected
        /// </summary>
        void LanguageMenuEntrySelected(object sender, EventArgs e)
        {
            // Check to see if the current language is an index greater than the total number of languages
            if (currentLanguage < languages.Length - 1)
            {
                // Increment it if it is not
                currentLanguage++;
            }
            else
            {
                // Set back to the beginning otherwise
                currentLanguage = 0;
            }

            // Update the menu entry text with the newly selected language
            SetMenuEntryText();
        }

        #endregion
    }
}
