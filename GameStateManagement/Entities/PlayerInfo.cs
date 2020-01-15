/*
 * Author: Shon Vivier
 * File Name: PlayerInfo.cs
 * Project Name: PASS2
 * Creation Date: 4/13/2019
 * Modified Date: 4/15/2019
 * Description: Stores all of the player's information statically
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateManagement.Entities
{
    static class PlayerInfo
    {
        // Define the player information
        public static int Wins = 0;

        public static int Losses = 0;

        public static int Score = 0;

        public static int[] Moves = { 0, 1, -1, -1 };
    }
}
