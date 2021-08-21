using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PonyLigaAPI.Models
{
    public class Pony
    {
        public int id { get; set; }
        public String name { get; set; }
        public String race { get; set; }
        public String age { get; set; }

        public ICollection<Team> teams { get; set; }
        public ICollection<TeamPony> teamPonies { get; set; }
    }
}
