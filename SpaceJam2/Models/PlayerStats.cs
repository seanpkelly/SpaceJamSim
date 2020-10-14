using System;
using System.Collections.Generic;

namespace SpaceJam2.Models
{
    public partial class PlayerStats
    {
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
        public double Points { get; set; }
        public double Assists { get; set; }
        public double Rebounds { get; set; }
        public double Blocks { get; set; }
        public double Steals { get; set; }
    }
}
