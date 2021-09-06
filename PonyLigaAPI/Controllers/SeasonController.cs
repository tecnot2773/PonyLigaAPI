using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PonyLigaAPI.Models;
using PonyLigaAPI.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace PonyLigaAPI.Controllers
{
    [ExcludeFromCodeCoverage] // NYI
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class SeasonController : ControllerBase
    {
        private readonly PonyLigaAPIContext _context;

        public SeasonController(PonyLigaAPIContext context)
        {
            _context = context;
        }

        // GET: api/Season
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Season>>> GetSeasons()
        {
            return await _context.Seasons.ToListAsync();
        }

        // GET: api/Season/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Season>> GetSeason(int id)
        {
            var season = await _context.Seasons.FindAsync(id);

            if (season == null)
            {
                return NotFound();
            }

            return season;
        }

        // PUT: api/Season/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeason(int id, Season season)
        {
            if (id != season.id)
            {
                return BadRequest();
            }

            _context.Entry(season).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeasonExists(id))
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

        // POST: api/Season
        [HttpPost]
        public async Task<ActionResult<Season>> PostSeason(Season season)
        {
            _context.Seasons.Add(season);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSeason", new { id = season.id }, season);
        }

        // DELETE: api/Season/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Season>> DeleteSeason(int id)
        {
            var season = await _context.Seasons.FindAsync(id);
            if (season == null)
            {
                return NotFound();
            }

            _context.Seasons.Remove(season);
            await _context.SaveChangesAsync();

            return season;
        }

        private bool SeasonExists(int id)
        {
            return _context.Seasons.Any(e => e.id == id);
        }
    }
}
