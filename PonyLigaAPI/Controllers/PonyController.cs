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
    public class PonyController : ControllerBase
    {
        private readonly PonyLigaAPIContext _context;

        public PonyController(PonyLigaAPIContext context)
        {
            _context = context;
        }

        // GET: api/Pony
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pony>>> GetPonies()
        {
            var ponies = await _context.Ponies.Include(e => e.teamPonies).ToListAsync();

            foreach (Pony pony in ponies)
            {
                foreach (TeamPony teamPony in pony.teamPonies)
                {
                    teamPony.pony = null;
                }
            }
            if (ponies == null)
            {
                return NotFound();
            }

            return ponies;

            //return await _context.Ponies.ToListAsync();
        }

        // GET: api/Pony/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pony>> GetPony(int id)
        {
            var pony = await _context.Ponies.Include(e => e.teamPonies).Where(e => e.id == id).FirstOrDefaultAsync();

            foreach (TeamPony teamPony in pony.teamPonies)
            {
                teamPony.pony = null;
            }

            if (pony == null)
            {
                return NotFound();
            }

            return pony;
        }

        // PUT: api/Pony/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPony(int id, Pony pony)
        {
            if (id != pony.id)
            {
                return BadRequest();
            }

            _context.Entry(pony).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PonyExists(id))
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

        // POST: api/Pony
        [HttpPost]
        public async Task<ActionResult<Pony>> PostPony(Pony pony)
        {
            _context.Ponies.Add(pony);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPony", new { id = pony.id }, pony);
        }

        // DELETE: api/Pony/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pony>> DeletePony(int id)
        {
            var pony = await _context.Ponies.FindAsync(id);
            if (pony == null)
            {
                return NotFound();
            }

            _context.Ponies.Remove(pony);
            await _context.SaveChangesAsync();

            return pony;
        }

        private bool PonyExists(int id)
        {
            return _context.Ponies.Any(e => e.id == id);
        }
    }
}
