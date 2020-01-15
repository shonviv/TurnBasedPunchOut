/*
 * Author: Shon Vivier
 * File Name: EnemyMatchInfo.cs
 * Project Name: PASS2
 * Creation Date: 4/15/2019
 * Modified Date: 4/15/2019
 * Description: This class displays all the information associated with a given opponenet.
 * Used for the Match Screen.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateManagement.Entities
{
    class EnemyMatchInfo
    {
        // Match information
        public string MatchImageName { get; set; }

        public string BoxingGym { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Record { get; set; }

        public int Ranking { get; set; }

        public int Weight { get; set; }

        public EnemyMatchInfo()
        {
            // Load different match information based on the players victories which define what enemy they face
            // Also add the player losses to their wins---this way, every time the player loses, it feels like
            // the enemy is gaining more wins on their pro record even if every enemy gets the same bonus.
            switch (PlayerInfo.Wins)
            {
                case 0:
                    MatchImageName = "MamoruAoyama";
                    BoxingGym = "Juvie";
                    FirstName = "Aoyama";
                    LastName = "Mamoru";
                    Record = $"{1 + PlayerInfo.Losses} - 2  {PlayerInfo.Losses}KO";
                    Ranking = 324;
                    Weight = 109;
                    break;
                case 1:
                    MatchImageName = "KanichiNishi";
                    BoxingGym = "Tange";
                    FirstName = "Nishi";
                    LastName = "Kinichi";
                    Record = $"{7 + PlayerInfo.Losses} - 3  {4 + PlayerInfo.Losses}KO";
                    Ranking = 51;
                    Weight = 191;
                    break;
                case 2:
                    MatchImageName = "WolfKanagushi";
                    BoxingGym = "Asia";
                    FirstName = "Wolf";
                    LastName = "Kanagushi";
                    Record = $"{10 + PlayerInfo.Losses} - 1  {7 + PlayerInfo.Losses}KO";
                    Ranking = 6;
                    Weight = 119;
                    break;
                case 3:
                    MatchImageName = "RikiishiToru";
                    BoxingGym = "Shiraki";
                    FirstName = "Rikiishi";
                    LastName = "Toru";
                    Record = $"{15 + PlayerInfo.Losses} - 0  {15 + PlayerInfo.Losses}KO";
                    Ranking = 1;
                    Weight = 118;
                    break;
            }
        }
    }
}
