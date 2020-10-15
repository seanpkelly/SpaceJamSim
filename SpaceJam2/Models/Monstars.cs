using System;
using System.Collections.Generic;

namespace SpaceJam2.Models
{
    public partial class Monstars:AbstractTeam
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Pound { get; set; }
        public string Bupkus { get; set; }
        public string Nawt { get; set; }
        public string Bang { get; set; }
        public string Blanko { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}
