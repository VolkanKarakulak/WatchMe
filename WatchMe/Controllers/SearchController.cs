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
        public ActionResult<List<Media>> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return BadRequest("Search Term Can't be Empty");
            }

            var mediaQuery = _context!.Medias
                .Include(m => m.MediaCategories)
                .Where(m => m.Name.Contains(searchTerm) || m.Description!.Contains(searchTerm));

            var mediaList = mediaQuery.ToList();

            return Ok(mediaList);
        }
    }
}



