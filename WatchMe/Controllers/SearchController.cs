using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using WatchMe.Data;
using WatchMe.Models;
using WatchMe.ViewModels;

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
        //[Authorize]
        public ActionResult<List<MediaDirectorViewModel>> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return BadRequest("Search Term Can't be Empty");
            }

            var mediaQuery = _context!.Medias
                .Include(m => m.MediaCategories!)
                    .ThenInclude(mc => mc.Category)
                .Include(m => m.MediaDirectors!)
                    .ThenInclude(md => md.Directors)
                .Include(m => m.MediaRestrictions!)
                    .ThenInclude(md => md.Restrictions)
                .Where(m => m.Name.Contains(searchTerm) || m.Description!.Contains(searchTerm));

            var mediaList = mediaQuery.ToList();

            var mediaViewModelList = mediaList.Select(media => new SearchMediaViewModel
            {
                MediaName = media.Name,
                Description = media.Description,
                IMDBRating = media.IMDBRating,
                Categories = media.MediaCategories?.Select(mc => mc.Category!.Name).ToList() ?? new List<string>(),
                Directors = media.MediaDirectors?.Select(md => md.Directors!.Name).ToList() ?? new List<string>(),
                Restriction = media.MediaRestrictions?.Select(mr => mr.Restrictions!.Name).ToList() ?? new List<string>()
            }).ToList();

            return Ok(mediaViewModelList);
        }

    }
}



