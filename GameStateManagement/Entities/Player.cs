/*
 * Author: Shon Vivier
 * File Name: Player.cs
 * Project Name: PASS2
 * Creation Date: 4/13/2019
 * Modified Date: 4/16/2019
 * Description: A class responsible for handling all of the player entities functions
*/

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GameStateManagement.Entities
{
    public class Player
    {
        // Define the player's MaxHP and CurrentHP
        public int MaxHP { get; set; }

        public int CurrentHP { get; set; }

        // Store the amount of damage the player gave and recieved

        public int DamageGiven { get; set; }

        public int DamageReceived { get; set; }

        // Define the information about the player's currently selected move
        public string Type { get; set; }

        public string[] ValueRange { get; set; }

        public int Chance { get; set; }
        
        public int Value { get; set; }

        // States whether or not the last used move was successful
        public bool MoveSuccessful { get; set; }

        // List of move names the player has
        public List<string> MoveNames { get; set; } = new List<string>();

        /// <summary>
        /// Constructor for the player class that takes in a content manager to load sound effects
        /// </summary>
        public Player(ContentManager content)
        {
            // Set the MaxHP and CurrentHP
            MaxHP = 100;
            CurrentHP = MaxHP;

            // Load the XML and select the move node under Moves
            moveSet.Load(@"..\..\Entities\MoveSet.xml");
            moveSetList = moveSet.SelectNodes("Moves");

            // Define the moveNode as the child of the root Moves element and cast moveNode as an XmlElement and store it as move
            moveNode = moveSetList[0];
            move = (XmlElement)moveNode;

            // Iterate over all of the moves the player has
            foreach (int moveIndex in PlayerInfo.Moves)
            {
                // Player is initialized afterwards, the player's moves are sorted in descending order.
                // Check to see if the move index is -1 and break if we exhaust all the moves
                if (moveIndex == -1)
                    break;

                // Add the move name to the list of move names
                MoveNames.Add(move.ChildNodes[moveIndex].Name);
            }

            // Load the attack and heal sound
            attackSnd = content.Load<SoundEffect>("Audio/SoundEffects/Attack");
            healSnd = content.Load<SoundEffect>("Audio/SoundEffects/Heal");
        }

        // Define the moveSet as an instance of XmlDocument and declare the moveSetList
        XmlDocument moveSet = new XmlDocument();
        XmlNodeList moveSetList;

        // Define a new random instance
        Random rand = new Random();

        // Declare an XmlNode and XmlElement
        XmlNode moveNode;
        XmlElement move;

        // Declare the sound effects the player uses
        SoundEffect attackSnd;
        SoundEffect healSnd;

        /// <summary>
        /// Handles attack-type moves which damage the enemy
        /// </summary>
        private void Attack(Enemy enemy, int damage)
        {
            // Play an attack sound
            attackSnd.Play();

            // Check to see if a player got a critical hit, and double the amount if they did
            if (rand.Next(1, 100) <= 10)
            {
                damage *= 2;
            }

            // Check to see if the damage the player will deal will be greater than
            // the enemy's health. If it is not, decrease the damage from the health.
            // Otherwise, set it to 0 (instead of negative)
            if ((enemy.CurrentHP - damage) > 0)
            { 
                enemy.CurrentHP -= damage;
                DamageGiven += damage;
                enemy.DamageReceived += damage;
                PlayerInfo.Score += damage * 2;
            }
            else
            {
                enemy.DamageReceived += damage - enemy.CurrentHP;
                DamageGiven += damage - enemy.CurrentHP;
                PlayerInfo.Score += (damage - enemy.CurrentHP) * 2;
                enemy.CurrentHP = 0;
            }
        }

        /// <summary>
        /// Handles heal-type moves which heal the player. All of the moves
        /// are either blocks or slips (which I would justify as canonically saving
        /// the stamina of the boxer, thereby healing them)
        /// </summary>
        private void Heal(int healAmount)
        {
            // Play the healing sound
            healSnd.Play();

            // Check to see if a player got a critical heal, and double the amount if they did
            if (rand.Next(1, 100) <= 10)
            {
                healAmount *= 2;
            }

            // Check to see if the healing amount the player will receive will be greater than
            // the player's health. If it is not, increase the player's health.
            // Otherwise, set it to the maximum amount the player can have.
            if ((CurrentHP + healAmount) > MaxHP)
            {
                PlayerInfo.Score += MaxHP - CurrentHP;
                CurrentHP = MaxHP;
            }
            else
            {
                PlayerInfo.Score += healAmount;
                CurrentHP += healAmount;
            }
        }

        /// <summary>
        /// Update the selected move information
        /// </summary>
        public void UpdateMove(int moveIndex)
        {
            // Set the properties to be the XML information of the selected move
            Type = move.GetElementsByTagName("Type")[moveIndex].InnerText;
            ValueRange = move.GetElementsByTagName("Range")[moveIndex].InnerText.Split('-');
            Chance = int.Parse(move.GetElementsByTagName("Chance")[moveIndex].InnerText);
            Chance = int.Parse(move.GetElementsByTagName("Chance")[moveIndex].InnerText);
        }

        /// <summary>
        /// Handles selecting the player's move
        /// </summary>
        public void SelectMove(Enemy enemy, int moveIndex, PlayerSpriteManager playerSpriteManager)
        {
            // Update the current move index
            UpdateMove(moveIndex);

            // Check to see if the player can even use the move
            if (CurrentHP <= 0)
            {
                return;
            }

            // Generate a number from 1 to 100 and see if the chance is greater
            if (rand.Next(1, 100) < Chance)
            {
                // The move was successful
                MoveSuccessful = true;

                // Set the value
                Value = rand.Next(int.Parse(ValueRange[0]), int.Parse(ValueRange[1]));

                // Handle attacks
                if (Type == "Attack")
                { 
                    Attack(enemy, Value);
                }
                // Handle healing
                else if (Type == "Heal")
                {
                    Heal(Value);
                    
                }
            }
            else
            {
                // The move was not successful---notify the game screen through the MoveSuccessful property
                MoveSuccessful = false;
            }

            // Change the player's animation to their current move
            playerSpriteManager.CurrentAnimation = move.ChildNodes[moveIndex].Name;
        }
    }
}
