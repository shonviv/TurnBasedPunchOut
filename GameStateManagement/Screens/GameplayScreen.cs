#region File Description
/*
 * Author: Shon Vivier
 * File Name: GameplayScreen.cs
 * Project Name: PASS2
 * Creation Date: 4/14/2019
 * Modified Date: 4/16/2019
 * Description: This screen implements the game logic for each match. This is where almost
 * all gameplay takes place.
*/
#endregion

#region Using Statements
using System;
using System.Threading;
using Animation2D;
using GameStateManagement.Entities;
using GameStateManagement.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace GameStateManagement
{
    class GameplayScreen : GameScreen
    {
        #region Fields

        // Define the buffer width and height we use
        int BufferWidth = 800;
        int BufferHeight = 450;

        // Set the maximum amount of time a sprite can be in an animation for
        float maxAnimTime = 0.5f;

        // Declare our selected move and maximum number of moves
        int selectedMove;
        int maxMoves;

        // Declare the content manager, primary font, and smaller font for move descriptions
        ContentManager content;
        SpriteFont gameFont;
        SpriteFont smallFont;
        
        // Declare our player and enemy entities
        Player player;
        Enemy enemy;

        // Declare a pause alpha for the amount of dimming in the puase menu
        float pauseAlpha;

        // Declare a rectangle with the dimensions of the full screen
        Rectangle fullScreenRec;
        Rectangle playerHealthBarRec;
        
        // Declare whether or not the match is over and it is the players turn
        bool matchOver;
        bool playerTurn;
        
        // Declare our background and move select foreground images
        Texture2D backgroundImg;
        Texture2D moveSelectImg;

        Texture2D playerHealthBarImg;

        // Declare our sprite managers for the enemy and player
        EnemySpriteManager enemySpriteManager;
        PlayerSpriteManager playerSpriteManager;

        // Retrieve the match info of the current enemy
        EnemyMatchInfo enemyMatchInfo;

        // Store the time since the enemy last attacked
        float timeSinceLastEnemyAnim = 0f;
        float timeSinceLastPlayerAnim = 0f;

        // Declare the current song playing
        Song track;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor for the Gameplay screen
        /// </summary>
        public GameplayScreen()
        {
            // Set the transition times
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

            // Define the fonts
            gameFont = ScreenManager.Font;
            smallFont = content.Load<SpriteFont>("small");

            // Set the selected move to 0 and match over to false
            selectedMove = 0;
            matchOver = false;
            playerTurn = true;

            // Sort and reverse our move array
            Array.Sort(PlayerInfo.Moves);
            Array.Reverse(PlayerInfo.Moves);

            // Iterate over each of the moves
            foreach (int move in PlayerInfo.Moves)
            {
                // Because our array is now sorted by descending order, whenver we hit the first -1,
                // we know that the following elements are less than or equal to -1, meaning they are 
                // unusuable. We can ignore the rest because they do not count as moves
                if (move == -1)
                    break;

                // Increment the maximum numer of moves
                maxMoves++;
            }

            // Load the background and move select foreground image
            backgroundImg = content.Load<Texture2D>("Images/Backgrounds/GameplayBackground");
            moveSelectImg = content.Load<Texture2D>("Images/Sprites/MoveSelectScreen");

            playerHealthBarImg = content.Load<Texture2D>("Images/Sprites/PlayerHealthBar");

            // Define the dimensions of the fullscreen rectangle
            fullScreenRec = new Rectangle(0, 0, BufferWidth, BufferHeight);
            playerHealthBarRec = new Rectangle(85, 30, playerHealthBarImg.Width, playerHealthBarImg.Height);

            // Initialize the player, enemy and enemy match info
            player = new Player(content);
            enemy = new Enemy();
            enemyMatchInfo = new EnemyMatchInfo();

            // Initialize the enemy and player sprite managers
            enemySpriteManager = new EnemySpriteManager(content, new Vector2(340, 60), enemyMatchInfo.FirstName);
            playerSpriteManager = new PlayerSpriteManager(content, new Vector2(360, 200));
            
            // Define and play the background track corresponding to the correct opponent
            track = content.Load<Song>($"Audio/Music/BattleThemes/{enemyMatchInfo.FirstName}");
            MediaPlayer.Play(track);

            // Use ResetElapsedTime to tell the game's timing mechanism that we have just finished a very long frame, and that it should not try to catch up.
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

            if (IsActive)
            {
                // Update each enemy animation
                foreach (Animation anim in enemySpriteManager.Animations.Values)
                {
                    anim.Update(gameTime);
                }
                // If the enemy is not in it's idle animation, keep track of how long it has been in this animation
                if (enemySpriteManager.CurrentAnimation != "Idle")
                {
                    timeSinceLastEnemyAnim += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                // If the enemy is in this animation for more than the maximum animation time and is not downed, reset the animation and time.
                if (timeSinceLastEnemyAnim > maxAnimTime && enemySpriteManager.CurrentAnimation != "Down")
                {
                    enemySpriteManager.CurrentAnimation = "Idle";
                    timeSinceLastEnemyAnim = 0f;
                }
                
                // Repeat the previous process with the player's sprites, using the variables corresponding to the player
                foreach (Animation anim in playerSpriteManager.Animations.Values)
                {
                    anim.Update(gameTime);
                }
                if (playerSpriteManager.CurrentAnimation != "Idle" && playerSpriteManager.CurrentAnimation != "Down")
                {
                    timeSinceLastPlayerAnim += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (timeSinceLastPlayerAnim > maxAnimTime)
                {
                    playerSpriteManager.CurrentAnimation = "Idle";
                    timeSinceLastPlayerAnim = 0f;
                }
                
                playerHealthBarRec.Width = (int)(playerHealthBarImg.Width * ((float)player.CurrentHP) / (float)player.MaxHP);
            }
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
            
            // Check to see if the game is paused (esc)
            if (input.IsPauseGame())
            {
                // Add the pause screen
                ScreenManager.AddScreen(new PauseMenuScreen());
            }
            // Check to see if a menu element was selected and ensure that the match is not over
            if (input.IsMenuSelect() && !matchOver)
            {
                // Swap the turn state
                playerTurn = !playerTurn;

                // Check if it is the player's turn
                if (playerTurn)
                { 
                    // Let the player select a move
                    player.SelectMove(enemy, PlayerInfo.Moves[selectedMove], playerSpriteManager);
                }
                else
                { 
                    // Select the move after the player has selected their move (Z pressed a second time)
                    enemy.SelectMove(player, enemySpriteManager);
                }

                // Check to see if the enemy is dead
                if (enemy.CurrentHP <= 0)
                {
                    // Increment the player wins and set the match to over
                    PlayerInfo.Wins += 1;
                    matchOver = true;

                    // Play the downed animation
                    enemySpriteManager.CurrentAnimation = "Down";
                }
                // Check to see if the player is dead
                else if (player.CurrentHP <= 0)
                {
                    // Increment the palyer losses and set the match to over
                    PlayerInfo.Losses += 1;
                    matchOver = true;

                    // Play the downed animation
                    playerSpriteManager.CurrentAnimation = "Down";
                    // Load enemy win anim
                }
            }
            // Check to see if the start button was pressed and ensure that the match is over
            if (input.IsStartButton() && matchOver)
            {
                // Load the training screen if there are still enemies to face
                if (PlayerInfo.Wins < 4)
                { 
                    LoadingScreen.Load(ScreenManager, false, new TrainingScreen());
                }
                // Otherwise, wipe the save data and go back to the main menu
                else
                {
                    SaveAndLoadGame.WipeSave();
                    LoadingScreen.Load(ScreenManager, false, new MainMenuScreen());
                }
            }

            // Check if the player is navigating the menu to the left
            if (input.IsMenuLeft())
            {
                // Decrement the selected move
                selectedMove--;

                // Wrap the selected move back around to end if it passes behind the start
                if (selectedMove < 0)
                    selectedMove = maxMoves - 1;
            }
            // Check if the player is nagivating the menu to the right
            if (input.IsMenuRight())
            {
                // Increment the selected move
                selectedMove++;

                // Wrap the selected move back to the beginning if it goes beyond the end
                if (selectedMove >= maxMoves)
                    selectedMove = 0;
            }
        }
        
        /// <summary>
        /// Draws the gameplay screen
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Define the sprite batch
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            // Begin drawing
            spriteBatch.Begin();

            // Draw the background and move select background
            spriteBatch.Draw(backgroundImg, fullScreenRec, Color.White);
            spriteBatch.Draw(moveSelectImg, fullScreenRec, Color.White);

            // Draw the enemy and player sprites
            enemySpriteManager.Animations[enemySpriteManager.CurrentAnimation].Draw(spriteBatch, Color.White, Animation.FLIP_NONE);
            playerSpriteManager.Animations[playerSpriteManager.CurrentAnimation].Draw(spriteBatch, Color.White, Animation.FLIP_NONE);
            
            // Store the actual text of the last move to use in calculating the offset
            string lastMoveText = "";

            // Iterate over each of the moves the player has
            foreach (string move in player.MoveNames)
            {
                // Save the string to be prefixed with a > if it is selected
                string first = "";

                // Declare the offset and counter-offset for the text
                Vector2 offset = new Vector2(0, 0);
                int counterOffset = 10;

                // Check to see if the current move is the selected move
                if (player.MoveNames.IndexOf(move) == selectedMove)
                {
                    // Set the first string to > to indicate that it is selected
                    first = ">";

                    // Define the horizontal offset and add the counter-offset to push the string back marginally farther
                    offset = gameFont.MeasureString(first);
                    offset.X -= counterOffset;
                }

                // Draw the string
                spriteBatch.DrawString(gameFont, $"{first + move}", new Vector2(180 + (gameFont.MeasureString(lastMoveText).X) - offset.X, 385), Color.White);

                // Add the last moves full text for calculating the offset for the next move
                lastMoveText += " " + move;
            }

            // Update the player's current move information
            player.UpdateMove(PlayerInfo.Moves[selectedMove]);

            // Draw the selected move information
            spriteBatch.DrawString(smallFont, $"{player.Type}: {player.ValueRange[0]}-{player.ValueRange[1]} damage. {player.Chance}% chance to hit.", new Vector2(155, 345), Color.White);
            
            // Draw player information
            spriteBatch.DrawString(smallFont, $"Player:", new Vector2(90, 195), Color.White);
            spriteBatch.DrawString(smallFont, $"Health: {player.CurrentHP}", new Vector2(90, 210), Color.White);
            spriteBatch.DrawString(smallFont, $"Damage Taken: {player.DamageReceived}", new Vector2(90, 225), Color.White);
            spriteBatch.DrawString(smallFont, $"Damage Given: {player.DamageGiven}", new Vector2(90, 240), Color.White);
            spriteBatch.DrawString(smallFont, $"Success: {player.MoveSuccessful}", new Vector2(90, 255), Color.White);

            // Draw enemy information
            spriteBatch.DrawString(smallFont, $"Enemy:", new Vector2(650, 195), Color.White);
            spriteBatch.DrawString(smallFont, $"Health: {enemy.CurrentHP}", new Vector2(590, 210), Color.White);
            spriteBatch.DrawString(smallFont, $"Damage Taken: {enemy.DamageReceived}", new Vector2(530, 225), Color.White);
            spriteBatch.DrawString(smallFont, $"Damage Given: {enemy.DamageGiven}", new Vector2(530, 240), Color.White);
            spriteBatch.DrawString(smallFont, $"Success: {enemy.MoveSuccessful}", new Vector2(530, 255), Color.White);
           
            // Draw score
            spriteBatch.DrawString(gameFont, $"Score: {PlayerInfo.Score}", new Vector2(310, 15), Color.Black);
            
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
