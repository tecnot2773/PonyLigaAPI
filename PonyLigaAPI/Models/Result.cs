using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PonyLigaAPI.Models
{
    public class Result
    {
        public int id { get; set; }
        public String gameDate { get; set; }
        public String game { get; set; }
        public int position { get; set; }
        public String finishingTime { get; set; }
        public String startingTime { get; set; }
        public String timeSum { get; set; }
        public int score { get; set; }
        public int teamId { get; set; }
        public Team team { get; set; }
    }
}
