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
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class UserController : ControllerBase
    {
        private readonly PonyLigaAPIContext _context;

        public UserController(PonyLigaAPIContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            var users = await _context.Users.ToListAsync();
            
            foreach(User user in users)
            {
                user.passwordHash = null;
            }

            return users;
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            user.passwordHash = null;

            if (user == null)
            {
                return NotFound();
            }
            
            return user;
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.id)
            {
                return BadRequest();
            }
            user.passwordHash = user.passwordEncrypt();
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            user.passwordHash = user.passwordEncrypt();
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            user.passwordHash = null;
            return CreatedAtAction("GetUser", new { id = user.id }, user);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            user.passwordHash = null;

            return user;
        }

        [HttpPost]
        [Route("~/api/userlogin")]
        public async Task<ActionResult<User>> LoginUser(User user)
        {
            var dbUser = await _context.Users.Where(u => u.loginName == user.loginName).FirstOrDefaultAsync();

            if (dbUser == null)
            {
                return Unauthorized();
            }

            if (dbUser.comparePassword(user.passwordHash))
            {
                dbUser.passwordHash = null;
                return dbUser;
            }
            return Unauthorized();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.id == id);
        }
    }
}
