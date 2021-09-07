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
            if (teamPonies == null || teamPonies.Count == 0)
            {
                return NotFound();
            }

            return teamPonies;

            //return await _context.TeamPonies.ToListAsync();
        }

        // GET: api/TeamPonies/5
        [HttpGet("{teamId}")]
        public async Task<ActionResult<TeamPony>> GetTeamPony(int teamId, int ponyId)
        {
            var teamPony = await _context.TeamPonies.Include(t => t.pony).Include(t => t.team).Where(e => e.ponyId == ponyId).Where(e => e.teamId == teamId).FirstOrDefaultAsync();

            if (teamPony == null)
            {
                return NotFound();
            }

            teamPony.team.teamPonies = null;
            teamPony.pony.teamPonies = null;


            return teamPony;
        }

        // POST: api/TeamPonies
        [HttpPost]
        public async Task<ActionResult<TeamPony>> PostTeamPony(TeamPony teamPony)
        {
            try
            {
                _context.TeamPonies.Add(teamPony);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Conflict();
            }

            return CreatedAtAction("GetTeamPony", new { teamId = teamPony.teamId, ponyId = teamPony.ponyId }, teamPony);
        }

        // DELETE: api/TeamPonies/5
        [HttpDelete("{teamId}")]
        public async Task<ActionResult<TeamPony>> DeleteTeamPony(int teamId, int ponyId)
        {
            var teamPony = await _context.TeamPonies.FindAsync(ponyId, teamId);
            if (teamPony == null)
            {
                return NotFound();
            }

            _context.TeamPonies.Remove(teamPony);
            await _context.SaveChangesAsync();

            return teamPony;
        }

    //    private bool TeamPonyExists(int teamId, int ponyId)
    //    {
    //        var teamPony = _context.TeamPonies.Where(e => e.ponyId == ponyId).Where(e => e.teamId == teamId).FirstOrDefault();
            
    //        if (teamPony != null)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
    }
}
