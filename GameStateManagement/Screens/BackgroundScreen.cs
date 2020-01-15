#region File Description
/*
 * Author: Shon Vivier
 * File Name: Background.cs
 * Project Name: PASS2
 * Creation Date: 4/12/2019
 * Modified Date: 4/13/2019
 * Description: The background screen sits behind all the other menu screens.
 * It draws a background image that remains fixed in place regardless of whatever 
 * transitions the screens on top of it may be doing
*/
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
#endregion

namespace GameStateManagement
{
    class BackgroundScreen : GameScreen
    {
        #region Fields

        // Declare the content manager and background texture
        ContentManager content;
        Texture2D backgroundTexture;

        // Declare the song used in the menu
        Song opening;

        #endregion

        #region Initialization
        /// <summary>
        /// Constructor for the background screen
        /// </summary>
        public BackgroundScreen()
        {
            // Defines the fade transition times
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            // Load the background texture
            backgroundTexture = content.Load<Texture2D>("Images/Backgrounds/MenuBackground");

            // Define the opening music and play it
            opening = content.Load<Song>("Audio/Music/Opening");
            MediaPlayer.Play(opening);
        }
        

        /// <summary>
        /// Unloads graphics content for this screen
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw
        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }


        /// <summary>
        /// Draws the background screen
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Define the spritebatch
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            // Define the rectangle that covers the entire screen
            Rectangle fullscreenRec = new Rectangle(0, 0, 800, 450);

            // Draw the background
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, fullscreenRec, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
            spriteBatch.End();
        }
        
        #endregion
    }
}
