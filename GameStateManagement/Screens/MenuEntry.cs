#region File Description
/*
 * Author: Shon Vivier
 * File Name: MenuEntry.cs
 * Project Name: PASS2
 * Creation Date: 4/13/2019
 * Modified Date: 4/15/2019
 * Description: A helper class that represents a single entry in a MenuScreen. By default this
 * merely draws the entry text string, but it can be customized to display menu entries in different 
 * ways. This also provides an event that will be raised when the menu entry is selected.
*/
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace GameStateManagement
{
    class MenuEntry
    {
        #region Fields
        
        // The text rendered for this entry.
        string text;

        // Tracks a fading selection effect on the entry. The entries transition out of the selection effect when they are deselected
        float selectionFade;
        
        // The position at which the entry is drawn. This is set by the MenuScreen each frame in Update.
        Vector2 position;

        #endregion

        #region Properties

        // Gets or sets the text of this menu entry
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        
        // Gets or sets the position at which to draw this menu entry
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        
        #endregion

        #region Events
        // Event raised when the menu entry is selected.
        public event EventHandler Selected;
        
        // Method for raising the Selected event.
        public void OnSelectEntry()
        {
            Selected(this, null);
        }
        
        #endregion

        #region Initialization
        /// <summary>
        /// Constructs a new menu entry with the specified text
        /// </summary>
        public MenuEntry(string text)
        {
            this.text = text;
        }
        
        #endregion

        #region Update and Draw
        /// <summary>
        /// Updates the menu entry
        /// </summary>
        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            // When the menu selection changes, entries gradually fade between
            // their selected and deselected appearance, rather than instantly
            // popping to the new state.
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            // Check to see if the menu entry is selected and update the fade depending on whether or not it was
            if (isSelected)
                selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
            else
                selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
        }
        
        /// <summary>
        /// Draws the menu entry. This can be overridden to customize the appearance
        /// </summary>
        public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            // Draw the entry in light blue if it is selected---otherwise, draw it in white
            Color color = isSelected ? Color.LightBlue : Color.White;

            // Pulsate the size of the selected menu entry
            double time = gameTime.TotalGameTime.TotalSeconds;
            
            // Define the pulsation equation
            float pulsate = (float)Math.Sin(time * 6) + 1;

            // Define the scale of the pulsation
            float scale = 1 + pulsate * 0.05f * selectionFade;

            // Modify the alpha to fade text out during transitions
            color *= screen.TransitionAlpha;

            // Draw text, centered on the middle of each line
            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            SpriteFont font = screenManager.Font;

            // Define the origin of the entry
            Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            // Draw the entry
            spriteBatch.DrawString(font, text, position, color, 0, origin, scale, SpriteEffects.None, 0);
        }


        /// <summary>
        /// Queries how much space this menu entry requires
        /// </summary>
        public virtual int GetHeight(MenuScreen screen)
        {
            return screen.ScreenManager.Font.LineSpacing;
        }


        /// <summary>
        /// Queries how wide the entry is, used for centering on the screen
        /// </summary>
        public virtual int GetWidth(MenuScreen screen)
        {
            return (int)screen.ScreenManager.Font.MeasureString(Text).X;
        }


        #endregion
    }
}
