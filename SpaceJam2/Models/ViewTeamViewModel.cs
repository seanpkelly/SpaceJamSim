using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceJam2.Models
{
    public class ViewTeamViewModel
    {
        public Dictionary<ToonSquad, List<PlayerStats>> TeamStats { get; set; }

        public string TeamNumber { get; set; }

    }
}
