#region File Description
/*
 * Author: Shon Vivier
 * File Name: TrainingScreen.cs
 * Project Name: PASS2
 * Creation Date: 4/14/2019
 * Modified Date: 4/16/2019
 * Description: This screen implements the logic behind changing the players moves and 
 * visualizes it in the Draw method. The player enters this state after every match.
*/
#endregion

#region Using Statements
using System;
using System.Linq;
using System.Threading;
using System.Xml;
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
    class TrainingScreen : GameScreen
    {
        #region Fields

        // Declare the buffer width and height
        int BufferWidth = 800;
        int BufferHeight = 450;

        // Declare the content manager we use for this screen
        ContentManager content;
        
        // Used for the pause state (esc) and defines how much we dim the current scene
        float pauseAlpha;
        
        // Create a new XML document and node list for our move set
        XmlDocument moveSet;
        XmlNodeList moveSetList;

        // Declare the move node we are currently on and the XML element for the move node
        XmlNode moveNode;
        XmlElement move;

        // Declare what our currently selected move is and what our maximum number of choices are
        int selectedMove;
        int maxMoves;

        // Declare the background texture and background music this screen uses
        Texture2D backgroundTexture;
        Song trainingMusic;

        // Declare the font
        SpriteFont gameFont;

        // A rectangle that corresponds to the screens dimensions
        Rectangle fullscreenRec;

        #endregion

        #region Initialization


        /// <summary>
        /// The constructor defines what transition times we use in and out of transitioning scenes
        /// </summary>
        public TrainingScreen()
        {
            // Set transition in time as 1.5 seconds and set transition off time as 0.5
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Loads the graphics content for the game
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            // Define the font we use
            gameFont = ScreenManager.Font;

            // Instantiate the move set as a new XML document and load it
            moveSet = new XmlDocument();
            moveSet.Load(@"..\..\Entities\MoveSet.xml");

            // Select the root level XML tag "Moves" as our move set list
            moveSetList = moveSet.SelectNodes("Moves");
            
            // Declare move node as the children of our Moves (all of the XML for the moves themselves) and cast it to an XML element as move
            moveNode = moveSetList[0];
            move = (XmlElement)moveNode;

            // Load the background texture
            backgroundTexture = content.Load<Texture2D>("Images/Backgrounds/TrainingBackground");

            // Set the selected move to be 0
            selectedMove = 0;

            // Iterate over all of the move nodes (any child of Moves) count
            for (int i = 0; i < moveNode.ChildNodes.Count; i++)
            {
                // Check if the player satisfies the minimum required wins
                if (int.Parse(move.GetElementsByTagName("Wins")[i].InnerText) <= PlayerInfo.Wins)
                {
                    // Increment the max moves so that the user may navigate all of the moves visible to them
                    maxMoves++;
                }
            }

            // Load and play the training music
            trainingMusic = content.Load<Song>("Audio/Music/Training");
            MediaPlayer.Play(trainingMusic);

            // Define the rectangle to fit the screen
            fullscreenRec = new Rectangle(0, 0, BufferWidth, BufferHeight);

            // Once the load has finished, we use ResetElapsedTime to tell the game's timing mechanism that we have just finished a very long frame, and that it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload graphics content used by the game.
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
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            
            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            // Check if the user's input is null and throw an exception if it is
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile
            KeyboardState keyboardState = input.CurrentKeyboardStates;

            // Checks to see if the user hit the pause button (esc)
            if (input.IsPauseGame())
            {
                // Add a new screen on top of our current screen to display the pause menu
                ScreenManager.AddScreen(new PauseMenuScreen());
            }

            // Check to see if our menu selection key was hit (Z)
            if (input.IsMenuSelect())
            {
                // Check to see if the player already has the move they are on in the selection screen equipped
                if (PlayerInfo.Moves.Contains(selectedMove))
                {
                    // Get the index of this move and remove it from the player's currently equipped moves
                    int index = Array.IndexOf(PlayerInfo.Moves, selectedMove);
                    PlayerInfo.Moves[index] = -1;
                }
                // Otherwise, if there is an open slot for equipping a new move
                else if (PlayerInfo.Moves.Contains(-1))
                {
                    // Get the index of the open slot and fill it with the new selected index
                    int index = Array.IndexOf(PlayerInfo.Moves, -1);
                    PlayerInfo.Moves[index] = selectedMove;
                }
            }
            // Check to see if the start button (enter) was pressed and if the player has any moves equipped (I.E anything with an index greater than -1)
            if (input.IsStartButton() && Array.Exists(PlayerInfo.Moves, selectedMove => selectedMove > -1))
            {
                // Load the next match
                LoadingScreen.Load(ScreenManager, false, new MatchupInfoScreen());
            }
            // Check to see if the up arrow key was hit
            if (input.IsMenuUp())
            {
                // Decrement the selected move
                selectedMove--;

                // Wrap the selected move around if the user goes beyond the minimum moves
                if (selectedMove < 0)
                    selectedMove = maxMoves - 1;
            }
            // Check to see if the down arrow key was hit
            if (input.IsMenuDown())
            {
                // Increment the selected move
                selectedMove++;

                // Wrap back around if the player goes beyond the maximum amount of moves
                if (selectedMove >= maxMoves)
                    selectedMove = 0;
            }
        }

        /// <summary>
        /// Draws the gameplay screen
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Define new sprite batch
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            // Begin drawing
            spriteBatch.Begin();
            
            // Draw the background
            spriteBatch.Draw(backgroundTexture, fullscreenRec, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            // Iterate over every move in our XML
            for (int i = 0; i < moveNode.ChildNodes.Count; i++)
            {
                // Check to see if our player satisfies the win requirements for the move
                if (int.Parse(move.GetElementsByTagName("Wins")[i].InnerText) <= PlayerInfo.Wins)
                {
                    // Define new strings that we append or preface our final text display with depending on whether or not the move is selected or equipped
                    string first = "";
                    string last = "";

                    // Check to see if the current move is selected
                    if (i == selectedMove)
                    {
                        // Add a < sign to denote that the player is on this move
                        last = "<";
                    }
                    // Check to see if the move is in the player's equipped moves
                    if (PlayerInfo.Moves.Contains(i))
                    {
                        // Add a > sign to denote that the player has this move equipped
                        first = ">";
                    }

                    // Draw out the move and whether or not it is selected/equipped.
                    spriteBatch.DrawString(gameFont, $"{first + move.ChildNodes[i].Name + last}", new Vector2(30, 30 + 30*i), Color.DarkRed);
                }
            }

            // Draw out the currently equipped move information (type, amount, chance, and description)
            spriteBatch.DrawString(gameFont, $"Type: {move.GetElementsByTagName("Type")[selectedMove].InnerText}", new Vector2(30, 300), Color.White);
            spriteBatch.DrawString(gameFont, $"Amount: {move.GetElementsByTagName("Range")[selectedMove].InnerText}", new Vector2(30, 330), Color.White);
            spriteBatch.DrawString(gameFont, $"Chance: {move.GetElementsByTagName("Chance")[selectedMove].InnerText}", new Vector2(30, 360), Color.White);
            spriteBatch.DrawString(gameFont, $"{move.GetElementsByTagName("Description")[selectedMove].InnerText}", new Vector2(30, 390), Color.White);

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
