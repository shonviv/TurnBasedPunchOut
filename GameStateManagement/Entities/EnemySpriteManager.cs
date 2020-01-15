/*
 * Author: Shon Vivier
 * File Name: EnemySpriteManager.cs
 * Project Name: PASS2
 * Creation Date: 4/15/2019
 * Modified Date: 4/15/2019
 * Description: This class loads and handles all of the animations the enemy has access to
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
    public class EnemySpriteManager
    {

        // Define the animations dictionary
        public Dictionary<string, Animation> Animations { get; set; } = new Dictionary<string, Animation>();

        // Define the current animation property
        public string CurrentAnimation { get; set; }

        public EnemySpriteManager(ContentManager content, Vector2 drawLoc, string enemyName)
        {
            // Load in all of the sprites for our animation
            idleImg = content.Load<Texture2D>($"Images/Sprites/{enemyName}/Idle");
            uppercutImg = content.Load<Texture2D>($"Images/Sprites/{enemyName}/Uppercut");
            hookImg = content.Load<Texture2D>($"Images/Sprites/{enemyName}/Hook");
            blockImg = content.Load<Texture2D>($"Images/Sprites/{enemyName}/Block");
            slipImg = content.Load<Texture2D>($"Images/Sprites/{enemyName}/Slip");
            crossImg = content.Load<Texture2D>($"Images/Sprites/{enemyName}/Cross");
            jabImg = content.Load<Texture2D>($"Images/Sprites/{enemyName}/Jab");
            downImg = content.Load<Texture2D>($"Images/Sprites/{enemyName}/Down");

            // Store the dimensions of each of the spritesheets
            int[] idleDimensions = new int[3];
            int[] uppercutDimensions = new int[3];
            int[] hookDimensions = new int[3];
            int[] blockDimensions = new int[3];
            int[] slipDimensions = new int[3];
            int[] crossDimensions = new int[3];
            int[] jabDimensions = new int[3];
            int[] downDimensions = new int[3];

            // Change them based on the enemy loaded
            if (enemyName == "Aoyama")
            {
                idleDimensions = new int[] { 12, 1, 12 };
                uppercutDimensions = new int[] { 4, 1, 4 };
                hookDimensions = new int[] { 4, 1, 4 };
                blockDimensions = new int[] { 5, 1, 5 };
                slipDimensions = new int[] { 2, 1, 2 };
                crossDimensions = new int[] { 6, 1, 6 };
                jabDimensions = new int[] { 3, 1, 3 };
                downDimensions = new int[] { 3, 1, 3 };
            }
            else if (enemyName == "Nishi")
            {
                idleDimensions = new int[] { 6, 1, 6 };
                uppercutDimensions = new int[] { 4, 1, 4 };
                hookDimensions = new int[] { 5, 1, 5 };
                blockDimensions = new int[] { 2, 1, 2 };
                slipDimensions = new int[] { 2, 1, 2 };
                crossDimensions = new int[] { 4, 1, 4 };
                jabDimensions = new int[] { 2, 1, 2 };
                downDimensions = new int[] { 4, 1, 4 };
            }
            else if (enemyName == "Wolf")
            {
                idleDimensions = new int[] { 3, 1, 3 };
                uppercutDimensions = new int[] { 3, 1, 3 };
                hookDimensions = new int[] { 3, 1, 3 };
                blockDimensions = new int[] { 2, 1, 2 };
                slipDimensions = new int[] { 2, 1, 2 };
                crossDimensions = new int[] { 3, 1, 3 };
                jabDimensions = new int[] { 2, 1, 2 };
                downDimensions = new int[] { 2, 1, 2 };
            }
            else if (enemyName == "Rikiishi")
            {
                idleDimensions = new int[] { 4, 1, 4 };
                uppercutDimensions = new int[] { 2, 1, 2 };
                hookDimensions = new int[] { 2, 1, 2 };
                blockDimensions = new int[] { 2, 1, 2 };
                slipDimensions = new int[] { 2, 1, 2 };
                crossDimensions = new int[] { 2, 1, 2 };
                jabDimensions = new int[] { 2, 1, 2 };
                downDimensions = new int[] { 6, 1, 6 };
            }

            // Add the animation to the animations dictionary
            Animations.Add("Idle", new Animation(idleImg, idleDimensions[0], idleDimensions[1], idleDimensions[2], 0, 0, Animation.ANIMATE_FOREVER, 30, drawLoc, 1.75f, true));
            Animations.Add("Uppercut", new Animation(uppercutImg, uppercutDimensions[0], uppercutDimensions[1], uppercutDimensions[2], 0, uppercutDimensions[2], Animation.ANIMATE_ONCE, 30, drawLoc, 1.75f, true));
            Animations.Add("Hook", new Animation(hookImg, hookDimensions[0], hookDimensions[1], hookDimensions[2], 0, hookDimensions[2], Animation.ANIMATE_ONCE, 30, drawLoc, 1.75f, true));
            Animations.Add("Block", new Animation(blockImg, blockDimensions[0], blockDimensions[1], blockDimensions[2], 0, blockDimensions[2], Animation.ANIMATE_ONCE, 30, drawLoc, 1.75f, true));
            Animations.Add("Slip", new Animation(slipImg, slipDimensions[0], slipDimensions[1], slipDimensions[2], 0, slipDimensions[2], Animation.ANIMATE_ONCE, 30, drawLoc, 1.75f, true));
            Animations.Add("Cross", new Animation(crossImg, crossDimensions[0], crossDimensions[1], crossDimensions[2], 0, crossDimensions[2], Animation.ANIMATE_ONCE, 30, drawLoc, 1.75f, true));
            Animations.Add("Jab", new Animation(jabImg, jabDimensions[0], jabDimensions[1], jabDimensions[2], 0, jabDimensions[2], Animation.ANIMATE_ONCE, 30, drawLoc, 1.75f, true));
            Animations.Add("Down", new Animation(downImg, downDimensions[0], downDimensions[1], downDimensions[2], 0, downDimensions[2], Animation.ANIMATE_ONCE, 30, drawLoc, 1.75f, true));

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