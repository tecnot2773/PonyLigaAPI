using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PonyLigaAPI.Models
{
    public class Season
    {
        public int id { get; set; }
        public String teamName { get; set; }
        public String club { get; set; }
        public int score { get; set; }
        public String year { get; set; }
        public int placement { get; set; }
    }
}
