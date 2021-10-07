using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PonyLigaAPI.Models;
using PonyLigaAPI.Attributes;

namespace PonyLigaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class TeamController : ControllerBase
    {
        private readonly PonyLigaAPIContext _context;

        public TeamController(PonyLigaAPIContext context)
        {
            _context = context;
        }

        // GET: api/Team
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
        {
            var teams = await _context.Teams.Include(e => e.teamPonies).Include(e => e.group).Include(e => e.results).Include(e => e.teamMembers).ToListAsync();

            if (teams == null || teams.Count == 0)
            {
                return NotFound();
            }

            foreach (Team team in teams)
            {
                foreach (TeamPony teamPony in team.teamPonies)
                {
                    teamPony.team = null;
                }
                if (team.@group != null)
                {
                    team.@group.teams = null;
                }
                foreach (Result result in team.results)
                {
                    result.team = null;
                }
                foreach (TeamMember teamMember in team.teamMembers)
                {
                    teamMember.team = null;
                }
            }

            return teams;
        }

        // GET: api/Team/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeam(int id)
        {
            var team = await _context.Teams.Include(e => e.teamPonies).Include(e => e.group).Include(e => e.results).Include(e => e.teamMembers).Where(e => e.id == id).FirstOrDefaultAsync();

            if (team == null)
            {
                return NotFound();
            }

            foreach (TeamPony teamPony in team.teamPonies)
            {
                teamPony.team = null;
            }

            foreach (Result result in team.results)
            {
                result.team = null;
            }

            if (team.group != null)
            {
                team.group.teams = null;
            }

            foreach (TeamMember teamMember in team.teamMembers)
            {
                teamMember.team = null;
            }

            return team;
        }

        // PUT: api/Team/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(int id, Team team)
        {
            if (id != team.id)
            {
                return BadRequest();
            }

            var groupId = team.groupId; // Need to cache the groupId since it would be unset because of it being an optional variable.
            _context.Entry(team).State = EntityState.Modified;
            team.groupId = groupId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Team
        [HttpPost]
        public async Task<ActionResult<Team>> PostTeam(Team team)
        {
            _context.Teams.Add(team);

            await _context.SaveChangesAsync();

            if (team.ponyId != null)
            {
                _context.TeamPonies.Add(new TeamPony
                {
                    ponyId = (int)team.ponyId,
                    teamId = team.id
                });
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }
            

            if (team.teamPonies != null)
            {
                foreach (TeamPony teamPony in team.teamPonies)
                {
                    teamPony.team.teamPonies = null;
                }
            }

            return CreatedAtAction("GetTeam", new { id = team.id }, team);
        }

        // DELETE: api/Team/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Team>> DeleteTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return team;
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.id == id);
        }
    }
}
