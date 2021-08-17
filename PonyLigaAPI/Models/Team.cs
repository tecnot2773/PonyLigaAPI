using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PonyLigaAPI.Models
{
    public class Team
    {
        public int id { get; set; }
        public string club { get; set; }
        public string name { get; set; }
        public string place { get; set; }
        public string consultor { get; set; }
        public int teamSize { get; set; }
    }
}
