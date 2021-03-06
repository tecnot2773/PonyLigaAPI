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
    public class TeamMemberController : ControllerBase
    {
        private readonly PonyLigaAPIContext _context;

        public TeamMemberController(PonyLigaAPIContext context)
        {
            _context = context;
        }

        // GET: api/TeamMember
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamMember>>> GetTeamMembers()
        {
            var teamMembers = await _context.TeamMembers.Include(e => e.team).ToListAsync();

            if (teamMembers == null || teamMembers.Count == 0)
            {
                return NotFound();
            }

            foreach (TeamMember teamMember in teamMembers)
            {
                if (teamMember.team != null)
                {
                    teamMember.team.teamMembers = null;
                }
            }

            return teamMembers;

        }

        // GET: api/TeamMember/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamMember>> GetTeamMember(int id)
        {
            var teamMember = await _context.TeamMembers.Include(e => e.team).Where(e => e.id == id).FirstOrDefaultAsync();

            if (teamMember == null)
            {
                return NotFound();
            }

            if (teamMember.team != null)
            {
                teamMember.team.teamMembers = null;
            }

            return teamMember;
        }

        // GET: api/Team/TeamMember
        [HttpGet]
        [Route("~/api/team/{id}/teammember")]
        public async Task<ActionResult<IEnumerable<TeamMember>>> GetTeamMembersByTeamId(int id)
        {
            var teamMembers = await _context.TeamMembers.Where(e => e.teamId == id).ToListAsync();

            if (teamMembers == null || teamMembers.Count == 0)
            {
                return NotFound();
            }

            foreach (TeamMember teamMember in teamMembers)
            {
                if (teamMember.team != null)
                {
                    teamMember.team.teamMembers = null;
                }
            }

            return teamMembers;

        }

        // PUT: api/TeamMember/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeamMember(int id, TeamMember teamMember)
        {
            if (id != teamMember.id)
            {
                return BadRequest();
            }

            _context.Entry(teamMember).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                teamMember.updateTeamMemberCount(_context);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamMemberExists(id))
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

        // POST: api/TeamMember
        [HttpPost]
        public async Task<ActionResult<TeamMember>> PostTeamMember(TeamMember teamMember)
        {
            _context.TeamMembers.Add(teamMember);
            await _context.SaveChangesAsync();

            teamMember.updateTeamMemberCount(_context);

            if (teamMember.team != null)
            {
                teamMember.team.teamMembers = null;
            }

            return CreatedAtAction("GetTeamMember", new { id = teamMember.id }, teamMember);
        }

        // DELETE: api/TeamMember/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TeamMember>> DeleteTeamMember(int id)
        {
            var teamMember = await _context.TeamMembers.FindAsync(id);
            if (teamMember == null)
            {
                return NotFound();
            }

            _context.TeamMembers.Remove(teamMember);
            await _context.SaveChangesAsync();

            teamMember.updateTeamMemberCount(_context);

            return teamMember;
        }

        private bool TeamMemberExists(int id)
        {
            return _context.TeamMembers.Any(e => e.id == id);
        }
    }
}
