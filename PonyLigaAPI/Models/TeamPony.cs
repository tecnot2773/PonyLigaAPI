using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PonyLigaAPI.Models
{
    public class TeamPony
    {

        public int teamId { get; set; }
        public Team team { get; set; }
        public int ponyId { get; set; }
        public Pony pony { get; set; }
    }
}
