using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PonyLigaAPI.Models
{
    public class TeamMember
    {
        public int id { get; set; }
        public String firstName { get; set; }
        public String surName { get; set; }
        public int teamId { get; set; }
        public Team team { get; set; }
    }
}
