using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PonyLigaAPI.Models
{
    public class Result
    {
        public int id { get; set; }
        public DateTime gameDate { get; set; }
        public String game { get; set; }
        public int position { get; set; }
        public String time { get; set; }
        public int score { get; set; }
        public int teamId { get; set; }
        public Team team { get; set; }
    }
}
