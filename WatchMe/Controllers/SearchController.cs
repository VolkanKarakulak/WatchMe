using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using WatchMe.Data;
using WatchMe.Models;

namespace WatchMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly WatchMeContext _context;
        public SearchController(WatchMeContext context)
        {
            _context = context;
        }

        [HttpPost("Search")]
        [Authorize]
        public async Task<ActionResult<List<Media>>> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return BadRequest("Search term cannot be empty");
            }

            // Search for media using the search term
            var mediaQuery = _context!.Medias
                .Include(m => m.MediaCategories)
                .Where(m => m.Name.Contains(searchTerm) || m.Description.Contains(searchTerm));

            // Apply filters based on user preferences or other criteria (optional)

            // Convert to a list of Media objects
            var mediaList = await mediaQuery.ToListAsync();

            return Ok(mediaList);
        }

    }
}
