#region File Description
/*
 * Author: Shon Vivier
 * File Name: LoadingScreen.cs
 * Project Name: PASS2
 * Creation Date: 4/14/2019
 * Modified Date: 4/16/2019
 * Description: The loading screen coordinates transitions between the menu system and the game itself. 
 * Normally one screen will transition off at the same time as the next screen is transitioning on,
 * but larger transitions can take a longer time to load their data
*/
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace GameStateManagement
{
    class LoadingScreen : GameScreen
    {
        // Activate a loading screen, which will transition on at the same time
        // The loading screen watches the state of the previous screens.
        // When it sees they have finished transitioning off, it activates the real
        // next screen, which may take a long time to load its data. The loading
        // screen will be the only thing displayed while this load is taking place.

        #region Fields
        
        // Boolean to toggle slow loading and to see if all other screens are gone
        bool loadingIsSlow;
        bool otherScreensAreGone;

        // An array of screens that details the screens needed to load
        GameScreen[] screensToLoad;

        #endregion

        #region Initialization
        /// <summary>
        /// The constructor is private because loading screens should
        /// be activated via the static Load method instead.
        /// </summary>
        private LoadingScreen(ScreenManager screenManager, bool loadingIsSlow, GameScreen[] screensToLoad)
        {
            // Define the fields
            this.loadingIsSlow = loadingIsSlow;
            this.screensToLoad = screensToLoad;

            // Set the transition time
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Activates the loading screen
        /// </summary>
        public static void Load(ScreenManager screenManager, bool loadingIsSlow, params GameScreen[] screensToLoad)
        {
            // Tell all the current screens to transition off
            foreach (GameScreen screen in screenManager.GetScreens())
                screen.ExitScreen();

            // Create and activate the loading screen
            LoadingScreen loadingScreen = new LoadingScreen(screenManager, loadingIsSlow, screensToLoad);
            screenManager.AddScreen(loadingScreen);
        }
        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the loading screen
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            
            // If all the previous screens have finished transitioning
            // off, it is time to actually perform the load
            if (otherScreensAreGone)
            {
                // Remove the current screen
                ScreenManager.RemoveScreen(this);

                // Iterate overall the screens we have to load
                foreach (GameScreen screen in screensToLoad)
                {
                    // Check if the screen is null
                    if (screen != null)
                    {
                        // If it is not, add it to the screen manager
                        ScreenManager.AddScreen(screen);
                    }
                }

                // Use ResetElapsedTime to tell the  game timing mechanism that we have just finished a very long frame, and that it should not try to catch up
                ScreenManager.Game.ResetElapsedTime();
            }
        }


        /// <summary>
        /// Draws the loading screen
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // If we are the only active screen, that means all the previous screens
            // must have finished transitioning off. We check for this in the Draw
            // method, rather than in Update, because it isn't enough just for the
            // screens to be gone: in order for the transition to look good we must
            // have actually drawn a frame without them before we perform the load
            if ((ScreenState == ScreenState.Active) && (ScreenManager.GetScreens().Length == 1))
            {
                // Set the other screens gone to true
                otherScreensAreGone = true;
            }

            // We display a loading message while that is going on if the loading is slow,
            // but the menus load very quickly, and it would look strange if we flashed this 
            // up for just a fraction of a second while returning from the game to the menus. 
            // This parameter tells us how long the loading is going to take, so we know whether
            // to bother drawing the message
            if (loadingIsSlow)
            {
                // Define our sprite batch and font
                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                SpriteFont font = ScreenManager.Font;

                // Define our loading message
                const string message = "Loading.....";

                // Define the center vector
                Vector2 center = new Vector2(350, 210);

                // Draw the text.
                spriteBatch.Begin();
                spriteBatch.DrawString(font, message, center, Color.White);
                spriteBatch.End();
            }
        }
        #endregion
    }
}
