#region File Description
/*
 * Author: Shon Vivier
 * File Name: MatchInfoScreen.cs
 * Project Name: PASS2
 * Creation Date: 4/15/2019
 * Modified Date: 4/16/2019
 * Description: This screen displys all of the information relevant to the match,
 * including the opponents height, weight, boxing gym, etc. It appears to preface the
 * match (gameplay)
*/
#endregion

#region Using Statements
using System;
using System.Threading;
using Animation2D;
using GameStateManagement.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace GameStateManagement
{
    class MatchupInfoScreen : GameScreen
    {
        #region Fields

        // Define the buffer height and width we operate with
        int BufferWidth = 800;
        int BufferHeight = 450;

        // Define the content manager and sprite font we use
        ContentManager content;
        SpriteFont gameFont;
        
        // Store a seperate pause alpha to dim the screen when the pause menu is active
        float pauseAlpha;

        // Create textures corresponding to the player and enemy's profiles
        Texture2D playerMatchImg;
        Texture2D enemyMatchImg;

        // Get the enemy's information
        EnemyMatchInfo enemyInfo;

        // Declare the music playing during this scene
        Song matchMusic;

        // Define the rectangles that the player and enemy profile pictures use
        Rectangle playerProfileRec;
        Rectangle enemyProfileRec;

        #endregion

        #region Initialization
        /// <summary>
        /// The constructor for the MatchInfoScreen class
        /// </summary>
        public MatchupInfoScreen()
        {
            // Set the transition fade times
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Load graphics content for the game
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            // Set the game font
            gameFont = ScreenManager.Font;

            // Declare a new instance of the enemy match info
            enemyInfo = new EnemyMatchInfo();

            // Load the enemy and player match profiles
            playerMatchImg = content.Load<Texture2D>("JoeYabuki");
            enemyMatchImg = content.Load<Texture2D>(enemyInfo.MatchImageName);

            // Define the rectangle dimensions
            playerProfileRec = new Rectangle(115, 270, 120, 120);
            enemyProfileRec = new Rectangle(535, 115, 120, 120);


            // Define the background music and play it
            matchMusic = content.Load<Song>("Audio/Music/Match");
            MediaPlayer.Play(matchMusic);

            // Use ResetElapsedTime to tell the game's timing mechanism that we have just finished a very long frame, and that it should not try to catch up
            ScreenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload graphics content used by the game
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }

        #endregion

        #region Update and Draw
        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            
            // Gradually fade in or out depending on whether we are covered by the pause screen
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile
            KeyboardState keyboardState = input.CurrentKeyboardStates;

            // Check if the game was paused (esc)
            if (input.IsPauseGame())
            {
                // Overlay the pause screen
                ScreenManager.AddScreen(new PauseMenuScreen());
            }

            // Check if the start button was hit (enter)
            if (input.IsStartButton())
            {
                // Move onto the match screen
                LoadingScreen.Load(ScreenManager, false, new GameplayScreen());
            }
        }
        
        /// <summary>
        /// Draws the gameplay screen
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Set the background colour to black
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);
            
            // Declare the sprite batch
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            // Begin drawing
            spriteBatch.Begin();
            
            // Draw the circuit text
            spriteBatch.DrawString(gameFont, "MINOR CIRCUIT", new Vector2(60, 30), Color.Orange);

            // Draw the player's boxing gym info and weight
            spriteBatch.DrawString(gameFont, "FROM", new Vector2(80, 85), Color.White);
            spriteBatch.DrawString(gameFont, "TANGE", new Vector2(90, 105), Color.White);
            spriteBatch.DrawString(gameFont, "BOXING", new Vector2(90, 125), Color.White);
            spriteBatch.DrawString(gameFont, "GYM", new Vector2(90, 145), Color.White);
            spriteBatch.DrawString(gameFont, "WEIGHT: 117", new Vector2(80, 205), Color.White);
            
            // Draw the player's profile
            spriteBatch.Draw(playerMatchImg, playerProfileRec, Color.White);
            
            // Draw the player's wins and name
            spriteBatch.DrawString(gameFont, $"{PlayerInfo.Wins} - {PlayerInfo.Losses}  {PlayerInfo.Wins}KO", new Vector2(80, 230), Color.White);
            spriteBatch.DrawString(gameFont, $"JOE YABUKI", new Vector2(80, 400), Color.White);
            
            // Draw the centre text
            spriteBatch.DrawString(gameFont, "VS.", new Vector2(370, 210), Color.Orange);
            spriteBatch.DrawString(gameFont, "PUSH", new Vector2(355, 350), Color.Orange);
            spriteBatch.DrawString(gameFont, "START!", new Vector2(345, 370), Color.Orange);

            // Draw the enemy ranking
            spriteBatch.DrawString(gameFont, $"RANKED #{enemyInfo.Ranking}", new Vector2(520, 20), Color.LightGreen);

            // Draw the enemy name
            spriteBatch.DrawString(gameFont, $"{enemyInfo.FirstName}", new Vector2(520, 50), Color.White);
            spriteBatch.DrawString(gameFont, $"{enemyInfo.LastName}", new Vector2(535, 75), Color.White);

            // Draw the enemy profile picture and record
            spriteBatch.Draw(enemyMatchImg, enemyProfileRec, Color.White);
            spriteBatch.DrawString(gameFont, $"{enemyInfo.Record}", new Vector2(510, 240), Color.White);

            // Draw the enemy's boxing gym
            spriteBatch.DrawString(gameFont, "FROM", new Vector2(520, 280), Color.White);
            spriteBatch.DrawString(gameFont, $"{enemyInfo.BoxingGym}", new Vector2(530, 300), Color.White);
            spriteBatch.DrawString(gameFont, "BOXING", new Vector2(530, 320), Color.White);
            spriteBatch.DrawString(gameFont, "GYM", new Vector2(530, 340), Color.White);

            // Draw the enemy's weight
            spriteBatch.DrawString(gameFont, $"WEIGHT: {enemyInfo.Weight}", new Vector2(510, 390), Color.White);
            
            // Stop drawing
            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
        #endregion
    }
}
