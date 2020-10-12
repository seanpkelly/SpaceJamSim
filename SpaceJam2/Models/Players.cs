using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceJam2.Models
{
    //public class Rootobject
    //{
    //    public Player[] data { get; set; }
    //    public Meta meta { get; set; }
    //}
    public class Meta
    {
        public int total_pages { get; set; }
        public int current_page { get; set; }
        public int next_page { get; set; }
        public int per_page { get; set; }
        public int total_count { get; set; }
    }
    public class Players
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string position { get; set; }
        //public int height_feet { get; set; }
        //public int height_inches { get; set; }
        //public int weight_pounds { get; set; }
        public Team team { get; set; }
    }
    public class Team
    {
        public int id { get; set; }
        public string abbreviation { get; set; }
        public string city { get; set; }
        public string conference { get; set; }
        public string division { get; set; }
        public string full_name { get; set; }
        public string name { get; set; }
    }
}
