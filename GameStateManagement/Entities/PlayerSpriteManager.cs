/*
 * Author: Shon Vivier
 * File Name: PlayerSpriteManager.cs
 * Project Name: GameStateManagement
 * Creation Date: 4/15/2019
 * Modified Date: 4/15/2019
 * Description: This class loads and handles all of the animations the player has access to
 * via the CurrentAnimation property which accesses the dictionary of Animations
*/

using Animation2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameStateManagement.Entities
{
    public class PlayerSpriteManager
    {
        // Define the animations dictionary
        public Dictionary<string, Animation> Animations { get; set; } = new Dictionary<string, Animation>();

        // Define the current animation property
        public string CurrentAnimation { get; set; }

        public PlayerSpriteManager(ContentManager content, Vector2 drawLoc)
        {
            // Load in all of the sprites for our animation
            idleImg = content.Load<Texture2D>("Images/Sprites/Player/Idle");
            uppercutImg = content.Load<Texture2D>("Images/Sprites/Player/Uppercut");
            hookImg = content.Load<Texture2D>("Images/Sprites/Player/Hook");
            blockImg = content.Load<Texture2D>("Images/Sprites/Player/Block");
            slipImg = content.Load<Texture2D>("Images/Sprites/Player/Slip");
            crossImg = content.Load<Texture2D>("Images/Sprites/Player/Cross");
            jabImg = content.Load<Texture2D>("Images/Sprites/Player/Jab");
            downImg = content.Load<Texture2D>("Images/Sprites/Player/Down");

            // Add the animation to the animations dictionary
            Animations.Add("Idle", new Animation(idleImg, 2, 1, 2, 0, 0, Animation.ANIMATE_FOREVER, 30, drawLoc, 1.75f, true));
            Animations.Add("Uppercut", new Animation(uppercutImg, 3, 1, 3, 0, 3, Animation.ANIMATE_ONCE, 30, drawLoc, 1.75f, true));
            Animations.Add("Hook", new Animation(hookImg, 3, 1, 3, 0, 3, Animation.ANIMATE_ONCE, 30, drawLoc, 1.75f, true));
            Animations.Add("Block", new Animation(blockImg, 2, 1, 2, 0, 2, Animation.ANIMATE_ONCE, 30, drawLoc, 1.75f, true));
            Animations.Add("Slip", new Animation(slipImg, 3, 1, 3, 0, 3, Animation.ANIMATE_ONCE, 30, drawLoc, 1.75f, true));
            Animations.Add("Cross", new Animation(crossImg, 3, 1, 3, 0, 3, Animation.ANIMATE_ONCE, 30, drawLoc, 1.75f, true));
            Animations.Add("Jab", new Animation(jabImg, 3, 1, 3, 0, 3, Animation.ANIMATE_ONCE, 30, drawLoc, 1.75f, true));
            Animations.Add("Down", new Animation(downImg, 3, 1, 3, 0, 3, Animation.ANIMATE_ONCE, 10, drawLoc, 1.75f, true));

            // Set the default current animation
            CurrentAnimation = "Idle";
        }

        // Define all of the textures we use in our animation
        Texture2D idleImg;
        Texture2D uppercutImg;
        Texture2D hookImg;
        Texture2D blockImg;
        Texture2D slipImg;
        Texture2D crossImg;
        Texture2D jabImg;
        Texture2D downImg;
    }
}