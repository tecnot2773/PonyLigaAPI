using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> updateTeamMemberCount(PonyLigaAPIContext _context)
        {
            var team = _context.Teams.Where(t => t.id == this.teamId).Include(e => e.teamMembers).First();

            int teamMemberCount = team.teamMembers.Count();

            team.teamSize = teamMemberCount;

            var status = _context.SaveChanges();

            return true;
        }
    }
}
