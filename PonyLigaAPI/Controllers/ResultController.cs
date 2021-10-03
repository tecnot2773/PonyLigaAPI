using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PonyLigaAPI.Models;
using PonyLigaAPI.Attributes;
using System.Globalization;
using Newtonsoft.Json;

namespace PonyLigaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class ResultController : ControllerBase
    {
        private readonly PonyLigaAPIContext _context;

        public ResultController(PonyLigaAPIContext context)
        {
            _context = context;
        }

        // GET: api/Result
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Result>>> GetResults()
        {
            var results = await _context.Results.Include(e => e.team).ToListAsync();

            if (results == null || results.Count == 0)
            {
                return NotFound();
            }

            foreach (Result result in results)
            {
                if (result.team != null)
                {
                    result.team.results = null;
                }
            }

            return results;
        }

        // GET: api/Result/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Result>> GetResult(int id)
        {
            var result = await _context.Results.Include(e => e.team).Where(e => e.id == id).FirstOrDefaultAsync();

            if (result == null)
            {
                return NotFound();
            }

            if (result.team != null)
            {
                result.team.results = null;
            }

            return result;
        }

        // GET: api/Team/TeamMember
        [HttpGet]
        [Route("~/api/result/summary")]
        public async Task<ActionResult<IEnumerable<Team>>> GetResultSummary()
        {
            var teams = await _context.Teams.ToListAsync();

            if (teams == null || teams.Count == 0)
            {
                return NotFound();
            }

            foreach (Team team in teams)
            {
                team.results = await _context.Results.Where(e => e.teamId == team.id).ToListAsync();

                foreach (Result result in team.results)
                {
                    result.team = null;
                    team.totalScore += result.score;
                }
            }

            List<Team> sortedTeams = teams.OrderByDescending(t => t.totalScore).ToList();

            for (int i = 0; i < sortedTeams.Count(); i++)
            {
                sortedTeams[i].place = i + 1;
            }

            return sortedTeams;

        }

        // PUT: api/Result/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResult(int id, Result result)
        {
            if (id != result.id)
            {
                return BadRequest();
            }

            _context.Entry(result).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResultExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var calculateScore = result.calculateScoreAsync(_context);
            return NoContent();
        }

        // POST: api/Result
        [HttpPost]
        public async Task<ActionResult<Result>> PostResult(Result result)
        {   
            _context.Results.Add(result);
            var status = await _context.SaveChangesAsync();
            
            var calculateScore = result.calculateScoreAsync(_context);

            return CreatedAtAction("GetResult", new { id = result.id }, result);
        }

        // DELETE: api/Result/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result>> DeleteResult(int id)
        {
            var result = await _context.Results.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            _context.Results.Remove(result);
            await _context.SaveChangesAsync();

            var calculateScore = result.calculateScoreAsync(_context);

            return result;
        }

        private bool ResultExists(int id)
        {
            return _context.Results.Any(e => e.id == id);
        }
    }
}
