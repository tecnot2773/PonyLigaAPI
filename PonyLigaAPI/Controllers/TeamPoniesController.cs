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
    public class TeamPoniesController : ControllerBase
    {
        private readonly PonyLigaAPIContext _context;

        public TeamPoniesController(PonyLigaAPIContext context)
        {
            _context = context;
        }

        // GET: api/TeamPonies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamPony>>> GetTeamPonies()
        {
            var teamPonies = await _context.TeamPonies.Include(t => t.pony).Include(t => t.team).ToListAsync();

            foreach (TeamPony teamPony in teamPonies)
            {
                teamPony.team.teamPonies = null;
                teamPony.pony.teamPonies = null;
            }
            if (teamPonies == null)
            {
                return NotFound();
            }

            return teamPonies;

            //return await _context.TeamPonies.ToListAsync();
        }

        // GET: api/TeamPonies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamPony>> GetTeamPony(int id)
        {
            var teamPony = await _context.TeamPonies.Include(t => t.pony).Include(t => t.team).FirstOrDefaultAsync();


            teamPony.team.teamPonies = null;
            teamPony.pony.teamPonies = null;

            if (teamPony == null)
            {
                return NotFound();
            }

            return teamPony;
        }

        // PUT: api/TeamPonies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeamPony(int id, TeamPony teamPony)
        {
            if (id != teamPony.ponyId)
            {
                return BadRequest();
            }

            _context.Entry(teamPony).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamPonyExists(id))
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

        // POST: api/TeamPonies
        [HttpPost]
        public async Task<ActionResult<TeamPony>> PostTeamPony(TeamPony teamPony)
        {
            _context.TeamPonies.Add(teamPony);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TeamPonyExists(teamPony.ponyId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTeamPony", new { id = teamPony.ponyId }, teamPony);
        }

        // DELETE: api/TeamPonies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TeamPony>> DeleteTeamPony(int id)
        {
            var teamPony = await _context.TeamPonies.FindAsync(id);
            if (teamPony == null)
            {
                return NotFound();
            }

            _context.TeamPonies.Remove(teamPony);
            await _context.SaveChangesAsync();

            return teamPony;
        }

        private bool TeamPonyExists(int id)
        {
            return _context.TeamPonies.Any(e => e.ponyId == id);
        }
    }
}
