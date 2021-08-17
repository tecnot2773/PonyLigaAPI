using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PonyLigaAPI.Models
{
    public class Group
    {
        public int id { get; set; }
        public String name { get; set; }
        public int rule { get; set; }
        public int teamId { get; set; }
        public int groupSize { get; set; }
        public String participants { get; set; }
    }
}
