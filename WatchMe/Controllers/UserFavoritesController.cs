using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchMe.Data;
using WatchMe.Identity.Data;
using WatchMe.Models;

namespace WatchMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFavoritesController : ControllerBase
    {
        private readonly WatchMeContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        public UserFavoritesController(SignInManager<AppUser> signInManager, WatchMeContext context)
        {
            _signInManager = signInManager;
            _context = context;
        }

        // GET: api/UserFavorites
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<UserFavorite>>> GetUserFavorites()
        //{
        //  if (_context.UserFavorites == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.UserFavorites.ToListAsync();
        //}

        [HttpGet("WithUserId")]
        public async Task<ActionResult<List<Media>>> GetUserFavorites(long userId)
        {
            var appUser = await _signInManager.UserManager.FindByIdAsync(userId.ToString());

            if (_context.UserFavorites == null)
            {
                return NotFound();
            }

            // Eager loading to fetch favorites with their associated media in one query
            var userFavorites = await _context.UserFavorites
             .Include(uf => uf.Media)
             .Where(uf => uf.UserId == appUser.Id).Select(uf => uf.Media!.Name).ToListAsync();

            if (userFavorites.Count == 0)
            {
                return NotFound(); 
            }

            return Ok(userFavorites); 
        }


        // GET: api/UserFavorites/5
        [HttpGet("{WitheMail}")]
        public async Task<ActionResult<IEnumerable<UserFavorite>>> GetUserFavorites(string eMail)
        {
            // Kullanıcıyı bul
            var appUser = await _signInManager.UserManager.FindByEmailAsync(eMail);

            if (appUser == null)
            {
                return NotFound("User Not Found");
            }

            // Kullanıcının favori medyalarını getir
            var userFavorites = await _context.UserFavorites
             .Include(uf => uf.Media)
             .Where(uf => uf.UserId == appUser.Id).Select(uf => uf.Media!.Name).ToListAsync();

            if (userFavorites == null || userFavorites.Count == 0)
            {
                return NotFound("The User's Favorite Media Couldn't Found");
            }

            return Ok(userFavorites);
        }

        // PUT: api/UserFavorites/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754


        // POST: api/UserFavorites
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserFavorite>> PostUserFavorite(UserFavorite userFavorite)
        {
          if (_context.UserFavorites == null)
          {
              return Problem("Entity set 'WatchMeContext.UserFavorites'  is null.");
          }
            _context.UserFavorites.Add(userFavorite);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserFavoriteExists(userFavorite.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserFavorite", new { id = userFavorite.UserId }, userFavorite);
        }

        // DELETE: api/UserFavorites/5
        [HttpDelete("{mediaId}")]
        [Authorize]
        public async Task<IActionResult> DeleteUserFavorite(int mediaId, long userId)
        {
            AppUser? appUser = _signInManager.UserManager.Users.Where(u => u.Id == userId).FirstOrDefault();

            if (_context.UserFavorites == null)
            {
                return NotFound();
            }

            var userFavorite = await _context.UserFavorites.Include(n => n.Media).FirstOrDefaultAsync(n => n.MediaId == mediaId && n.UserId == appUser!.Id);

            if (userFavorite == null)
            {
                return NotFound("Favorite Media Not Found");
            }

            _context.UserFavorites.Remove(userFavorite);
            await _context.SaveChangesAsync();

            return Ok("Favorite Media Deleted");
        }

        private bool UserFavoriteExists(long id)
        {
            return (_context.UserFavorites?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
