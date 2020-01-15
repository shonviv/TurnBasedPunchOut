/*
 * Author: Shon Vivier
 * File Name: SaveAndLoadGame.cs
 * Project Name: PASS2
 * Creation Date: 4/16/2019
 * Modified Date: 4/16/2019
 * Description: The SaveAndLoadGame handles simple saving a loading for a game.
*/

using GameStateManagement.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateManagement.Managers
{
    public static class SaveAndLoadGame
    {
        /// <summary>
        /// Handles saving the game. Writes to a local file
        /// </summary>
        public static void SaveGame()
        {
            // Store the player info into an array of strings
            string[] lines = { PlayerInfo.Wins.ToString(), PlayerInfo.Losses.ToString(), string.Join(",", PlayerInfo.Moves), PlayerInfo.Score.ToString() };

            // Write to the local file
            File.WriteAllLines(@"../../Saves/Save.txt", lines);
        }

        /// <summary>
        /// Handles loading the game. Writes to a local file
        /// </summary>
        public static void LoadGame()
        {
            // Read in all the lines from the save file
            string[] lines = File.ReadAllLines(@"../../Saves/Save.txt");

            // Set the player info as the saved content
            PlayerInfo.Wins = int.Parse(lines[0]);
            PlayerInfo.Losses = int.Parse(lines[1]);
            PlayerInfo.Moves = Array.ConvertAll(lines[2].Split(','), s => int.Parse(s));
            PlayerInfo.Score = int.Parse(lines[3]);
        }

        /// <summary>
        /// Wipes the local save data
        /// </summary>
        public static void WipeSave()
        {
            // Override the current save and set it to default values
            File.WriteAllLines(@"../../Saves/Save.txt", new string[] { "0" , "0", "0,1,-1,-1", "0"});
        }
    }
}
