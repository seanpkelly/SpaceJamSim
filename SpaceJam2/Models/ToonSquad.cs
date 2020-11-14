using System;
using System.Collections.Generic;

namespace SpaceJam2.Models
{
    public partial class ToonSquad
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public string Player3 { get; set; }
        public string Player4 { get; set; }
        public string Player5 { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
