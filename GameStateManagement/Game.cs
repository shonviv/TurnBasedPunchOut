#region File Description
/*
 * Author: Shon Vivier
 * File Name: Game.cs
 * Project Name: PASS2
 * Creation Date: 4/12/2019
 * Modified Date: 4/14/2019
 * Description: Game.cs is the entry point for the main menu screen.
*/
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace GameStateManagement
{
    public class GameStateManagementGame : Game
    {
        #region Fields

        // Declare graphics and window height/width
        GraphicsDeviceManager graphics;

        int BufferWidth = 800;
        int BufferHeight = 450;

        // Define screenManager to access the ScreenManager classes
        ScreenManager screenManager;
        #endregion

        #region Initialization


        /// <summary>
        /// The main game constructor
        /// </summary>
        public GameStateManagementGame()
        {
            Content.RootDirectory = "Content";

            // Declare the graphics and define the preffered buffer width and height
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = BufferWidth;
            graphics.PreferredBackBufferHeight = BufferHeight;

            // Create the screen manager component
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            // Activate the first screens
            screenManager.AddScreen(new BackgroundScreen());
            screenManager.AddScreen(new MainMenuScreen());
        }


        #endregion

        #region Draw


        /// <summary>
        /// This method is called when the game should draw itself
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            // The real drawing happens inside the screen manager component
            base.Draw(gameTime);
        }


        #endregion
    }
}
