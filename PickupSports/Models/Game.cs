using System;
using System.Collections.Generic;
using System.Text;

namespace PickupSports.Models
{
    public class Game
    {
        public string timeDate { get; set; }

        public string gameLocation { get; set; }

        public string team1Name { get; set; }

        public string team2Name { get; set; }

        public int team1Score { get; set; }

        public int team2Score { get; set; }

        public string outcome { get; set; }
    }
}
