#region File Description
/*
 * Author: Shon Vivier
 * File Name: ScreenManager.cs
 * Project Name: PASS2
 * Creation Date: 4/14/2019
 * Modified Date: 4/16/2019
 * Description: The screen manager is a component which manages one or more GameScreen
 * instances. It maintains a stack of screens, calls their Update and Draw methods at 
 * the appropriate times, and automatically routes input to the topmost active screen.
*/
#endregion

#region Using Statements
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
#endregion

namespace GameStateManagement
{
    public class ScreenManager : DrawableGameComponent
    {
        #region Fields

        // Define the list of screens and screens we need to update
        List<GameScreen> screens = new List<GameScreen>();
        List<GameScreen> screensToUpdate = new List<GameScreen>();

        // Declare the input state for controlling input
        InputState input = new InputState();

        // Declare the sprite batch and font
        SpriteBatch spriteBatch;
        SpriteFont font;

        // Declare a blank texture
        Texture2D blankTexture;

        // Declare whether or not the screen is initialized
        bool isInitialized;

        #endregion

        #region Properties
        /// <summary>
        /// A default SpriteBatch shared by all the screens. This saves
        /// each screen having to bother creating their own local instance
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }


        /// <summary>
        /// A default font shared by all the screens. This saves
        /// each screen having to bother loading their own local copy
        /// </summary>
        public SpriteFont Font
        {
            get { return font; }
        }

        #endregion

        #region Initialization
        /// <summary>
        /// Constructs a new screen manager component
        /// </summary>
        public ScreenManager(Game game) : base(game) { }
        
        /// <summary>
        /// Initializes the screen manager component
        /// </summary>
        public override void Initialize()
        {
            // Initialize the screen manager and set isInitialized to true
            base.Initialize();
            isInitialized = true;
        }
        
        /// <summary>
        /// Load graphics content
        /// </summary>
        protected override void LoadContent()
        {
            // Load content belonging to the screen manager
            ContentManager content = Game.Content;

            // Define sprite batch, font and load the blank texture
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = content.Load<SpriteFont>("Pixel_NES");
            blankTexture = content.Load<Texture2D>("blank");

            // Tell each of the screens to load their content
            foreach (GameScreen screen in screens)
            {
                // Load the content for each screen
                screen.LoadContent();
            }
        }
        
        /// <summary>
        /// Unload your graphics content
        /// </summary>
        protected override void UnloadContent()
        {
            // Tell each of the screens to unload their content
            foreach (GameScreen screen in screens)
            {
                // Unload all the content in each screen
                screen.UnloadContent();
            }
        }

        #endregion

        #region Update and Draw
        /// <summary>
        /// Allows each screen to run logic
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // Read the keyboard
            input.Update();

            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others
            screensToUpdate.Clear();

            // Add each screen to update 
            foreach (GameScreen screen in screens)
                screensToUpdate.Add(screen);

            // Define otherScreenHasFocus as whether or not the game is not active and set coveredByOtherScreens to false
            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            // Loop as long as there are screens waiting to be updated
            while (screensToUpdate.Count > 0)
            {
                // Pop the topmost screen off the waiting list
                GameScreen screen = screensToUpdate[screensToUpdate.Count - 1];

                screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                // Update the screen
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                // Check to see if the screen state is transitioning
                if (screen.ScreenState == ScreenState.TransitionOn || screen.ScreenState == ScreenState.Active)
                {
                    // If this is the first active screen we came across,
                    // give it a chance to handle input
                    if (!otherScreenHasFocus)
                    {
                        // Set other screen has focus to true
                        screen.HandleInput(input);
                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it
                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }
        }

        /// <summary>
        /// Tells each screen to draw itself
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Iterate over all the screens
            foreach (GameScreen screen in screens)
            {
                // Skip the screen if its state is set to hidden
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;

                // Draw the screen
                screen.Draw(gameTime);
            }
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Adds a new screen to the screen manager
        /// </summary>
        public void AddScreen(GameScreen screen)
        {
            // Set the screen manager and isExiting
            screen.ScreenManager = this;
            screen.IsExiting = false;

            // If we have a graphics device, tell the screen to load content
            if (isInitialized)
            {
                screen.LoadContent();
            }

            // Add the screen
            screens.Add(screen);
        }


        /// <summary>
        /// Removes a screen from the screen manager. You should normally
        /// use GameScreen.ExitScreen instead of calling this directly, so
        /// the screen can gradually transition off rather than just being
        /// instantly removed
        /// </summary>
        public void RemoveScreen(GameScreen screen)
        {
            // If we have a graphics device, tell the screen to unload content
            if (isInitialized)
            {
                screen.UnloadContent();
            }

            // Remove the screens
            screens.Remove(screen);
            screensToUpdate.Remove(screen);
        }


        /// <summary>
        /// Expose an array holding all the screens. We return a copy rather
        /// than the real master list, because screens should only ever be added
        /// or removed using the AddScreen and RemoveScreen methods
        /// </summary>
        public GameScreen[] GetScreens()
        {
            return screens.ToArray();
        }


        /// <summary>
        /// Helper draws a translucent black fullscreen sprite, used for fading
        /// screens in and out, and for darkening the background behind popups
        /// </summary>
        public void FadeBackBufferToBlack(float alpha)
        {
            Viewport viewport = GraphicsDevice.Viewport;

            // Draw the fade texture
            spriteBatch.Begin();
            spriteBatch.Draw(blankTexture, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.Black * alpha);
            spriteBatch.End();
        }
        
        #endregion
    }
}
