using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PonyLigaAPI.Models
{
    public class Result
    {
        public int id { get; set; }
        public DateTime gameDate { get; set; }
        public String game { get; set; }
        public int position { get; set; }
        public String time { get; set; }
        public int score { get; set; }
        public int teamId { get; set; }
        public Team team { get; set; }

        public async Task<bool> calculateScoreAsync(PonyLigaAPIContext _context)
        {
            var results = _context.Results.Where(e => e.game == this.game).ToList();
            // Remove dots so we can convert time to int for sorting
            foreach (Result result in results)
            {
                result.time = result.time.Replace(":", "");
                result.time = result.time.Replace(".", "");
            }
            // Order Model list by time
            var sortedList = results.OrderBy(r => int.Parse(r.time)).ToList();

            // Place 1 gets #ofTeams as Score
            // Place 2 gets #ofTeams -1 as Score
            var score = results.Count;
            var position = 1;
            foreach (Result result in sortedList)
            {
                result.score = score;
                result.position = position;

                position++;
                score--;

                _context.Entry(result).Property(e => e.score).IsModified = true;
                _context.Entry(result).Property(e => e.position).IsModified = true;
                _context.Entry(result).Property(e => e.time).IsModified = false;
            }
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
