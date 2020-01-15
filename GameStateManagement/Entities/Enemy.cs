/*
 * Author: Shon Vivier
 * File Name: Player.cs
 * Project Name: PASS2
 * Creation Date: 4/13/2019
 * Modified Date: 4/16/2019
 * Description: A class responsible for handling all of the enemy entities functions
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace GameStateManagement.Entities
{
    public class Enemy
    {
        // Define the player's MaxHP and CurrentHP
        public int MaxHP { get; set; }

        public int CurrentHP { get; set; }

        // Store the amount of damage the player gave and recieved
        public int DamageGiven { get; set; }

        public int DamageReceived { get; set; }

        // States whether or not the last used move was successful
        public bool MoveSuccessful { get; set; }

        public Enemy()
        {
            // Generate MaxHP based on the number of wins a player has (harder difficulty for each successive enemy)
            MaxHP = rand.Next(50 * (PlayerInfo.Wins + 1), 70 * (PlayerInfo.Wins + 1));
            CurrentHP = MaxHP;

            // Load the XML and select the move node under Moves
            moveSet.Load(@"..\..\Entities\MoveSet.xml");
            moveSetList = moveSet.SelectNodes("Moves");

            // Define the moveNode as the child of the root Moves element and cast moveNode as an XmlElement and store it as move
            moveNode = moveSetList[0];
            move = (XmlElement)moveNode;
        }

        // Define the moveSet as an instance of XmlDocument and declare the moveSetList
        XmlDocument moveSet = new XmlDocument();
        XmlNodeList moveSetList;

        // Define a new random instance
        Random rand = new Random();

        // Declare an XmlNode and XmlElement
        XmlNode moveNode;
        XmlElement move;


        /// <summary>
        /// Handles attack-type moves which damage the player
        /// </summary>
        private void Attack(Player player, int damage)
        {
            // Check to see if a player got a critical hit, and double the amount if they did
            if (rand.Next(1, 100) <= 10)
            {
                damage *= 2;
            }

            // Check to see if the damage the enemy will deal will be greater than
            // the player's health. If it is not, decrease the damage from the health.
            // Otherwise, set it to 0 (instead of negative)
            if ((player.CurrentHP - damage) > 0)
            {
                PlayerInfo.Score -= damage;
                player.CurrentHP -= damage;
                DamageGiven += damage;
                player.DamageReceived += damage;
            }
            else
            {
                PlayerInfo.Score -= damage - player.CurrentHP;
                player.DamageReceived += damage - player.CurrentHP;
                DamageGiven += damage - player.CurrentHP;
                player.CurrentHP = 0;
            }
        }

        /// <summary>
        /// Handles heal-type moves which heal the enemy. All of the moves
        /// are either blocks or slips (which I would justify as canonically saving
        /// the stamina of the boxer, thereby healing them)
        /// </summary>
        private void Heal(int healAmount)
        {
            // Check to see if a player got a critical heal, and double the amount if they did
            if (rand.Next(1, 100) <= 10)
            {
                healAmount *= 2;
            }

            // Check to see if the healing amount the enemy will receive will be greater than
            // the enemy's health. If it is not, increase the enemy's health.
            // Otherwise, set it to the maximum amount the enemy can have.
            if ((CurrentHP + healAmount) > MaxHP)
            {
                CurrentHP = MaxHP;
            }
            else
            {
                CurrentHP += healAmount;
            }
        }

        /// <summary>
        /// Handles selecting the enemy's move
        /// </summary>
        public void SelectMove(Player player, EnemySpriteManager enemySpriteManager)
        {
            // Check to see if the player can even use the move
            if (CurrentHP <= 0)
            { 
                return;
            }
            
            // Set the default index to 3 (a slip with a type heal)
            int selectedIndex = 4;

            // Check if the enemy's health is above 25%---if it is, select a purely random move. Otherwise, stick with a healing move
            if (CurrentHP > MaxHP * 0.25)
            { 
                selectedIndex = rand.Next(0, moveNode.ChildNodes.Count);
            }

            // Get move information from XML
            string type = move.GetElementsByTagName("Type")[selectedIndex].InnerText;
            string[] valueRange = move.GetElementsByTagName("Range")[selectedIndex].InnerText.Split('-');
            int chance = int.Parse(move.GetElementsByTagName("Chance")[selectedIndex].InnerText);
            int value = rand.Next(int.Parse(valueRange[0]), int.Parse(valueRange[1]));

            // Generate a number from 1 to 100 and see if the chance is greater
            if (rand.Next(1, 100) < chance)
            {
                // The move was successful
                MoveSuccessful = true;

                // Handle attacks
                if (type == "Attack")
                {
                    Attack(player, value);
                }
                // Handle healing
                else if (type == "Heal")
                {
                    Heal(value);
                }
            }
            else
            {
                // The move was not successful---notify the game screen through the MoveSuccessful property
                MoveSuccessful = false;
            }

            // Change the player's animation to their current move
            enemySpriteManager.CurrentAnimation = move.ChildNodes[selectedIndex].Name;
        }
    }
}