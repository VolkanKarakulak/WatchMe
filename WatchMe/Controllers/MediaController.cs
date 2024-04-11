using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchMe.Data;
using WatchMe.Models;
using WatchMe.ViewModels;

namespace WatchMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly WatchMeContext _context;

        public MediaController(WatchMeContext context)
        {
            _context = context;
        }

        // GET: api/Media
        [HttpGet]
        [Authorize]
        public ActionResult<List<MediaGetViewModel>> GetMedias()
        {
            var  medialist = _context!.Medias
                .Include(x => x.MediaRestrictions!).ThenInclude(x => x.Restrictions)
                .Include(x => x.MediaCategories!).ThenInclude(x => x.Category)
                .Include(x => x.MediaDirectors!).ThenInclude(x => x.Directors)
                .Include(x => x.MediaStars!).ThenInclude(x => x.Star)
                .AsNoTracking()
                .Select(m => new MediaGetViewModel
                {
                    MediaName = m.Name,
                    Description = m.Description,
                    IsPassive = m.Passive,
                    IMDBRating = m.IMDBRating,
                    RestrictionNames = m.MediaRestrictions!.Select(mr => mr.Restrictions!.Name).ToList(),
                    CategoryNames = m.MediaCategories!.Select(mc => mc.Category!.Name).ToList(),
                    DirectorNames = m.MediaDirectors!.Select(md => md.Directors!.Name).ToList(),
                    StarNames = m.MediaStars!.Select(ms => ms.Star!.Name).ToList()
                }).ToList();

            return medialist;
        }

        // GET: api/Media/5
        [HttpGet("{id}")]
        //[Authorize]
        public ActionResult<MediaGetViewModel> GetMedia(int id)
        {
            Media? media = _context.Medias.Find(id);
            List<MediaRestriction>? mediaRestrictions = _context.MediaRestrictions.Where(u => u.MediaId == id).Include(u => u.Restrictions).ToList();
            List<MediaCategory>? mediaCategory = _context.MediaCategories.Where(u => u.MediaId == id).Include(u => u.Category).ToList();
            List<MediaDirector>? mediaDirector = _context.MediaDirectors.Where(d => d.MediaId == id).Include(d => d.Directors).ToList();
            List<MediaStar>? mediaStar = _context.MediaStars.Where(d => d.MediaId == id).Include(d => d.Star).ToList();

            if (media == null)
            {
                return NotFound();
            }

            var viewModel = new MediaGetViewModel
            {
                MediaName = media.Name,
                Description = media.Description,
                IsPassive = media.Passive,
                IMDBRating = media.IMDBRating,
                RestrictionNames = mediaRestrictions?.Select(mr => mr.Restrictions!.Name).ToList() ?? new List<string>(),
                CategoryNames = mediaCategory?.Select(mc => mc.Category!.Name).ToList() ?? new List<string>(),
                DirectorNames = mediaDirector?.Select(md => md.Directors!.Name).ToList() ?? new List<string>(),
                StarNames = mediaStar?.Select(ms => ms.Star!.Name).ToList() ?? new List<string>()
            };

            return viewModel;
        }
        // PUT: api/Media/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public ActionResult PutMedia(int id, Media media)
        {
            if (id != media.Id)
            {
                return BadRequest();
            }

            _context.Medias.Update(media);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            return Ok();
        }

        // POST: api/Media
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public int PostMedia(MediaPostViewModel MViewModel)
        {
            _context.Medias.Add(MViewModel.Media!);
            _context.SaveChanges();

            if (MViewModel.StarIds != null)
            {
                foreach (int starId in MViewModel.StarIds)
                {
                    MediaStar mediaStar = new MediaStar
                    {
                        StarId = starId,
                        MediaId = MViewModel.Media!.Id
                    };
                    _context.MediaStars.Add(mediaStar);
                }
            }

            if (MViewModel.CategoryIds != null)
            {
                foreach (short categoryId in MViewModel.CategoryIds)
                {
                    MediaCategory mediaCategory = new MediaCategory
                    {
                        CategoryId = categoryId,
                        MediaId = MViewModel.Media!.Id
                    };
                    _context.MediaCategories.Add(mediaCategory);
                }
            }

            if (MViewModel.DirectorIds != null)
            {
                foreach (int directorId in MViewModel.DirectorIds)
                {
                    MediaDirector mediaDirector = new MediaDirector
                    {
                        DirectorId = directorId,
                        MediaId = MViewModel.Media!.Id
                    };
                    _context.MediaDirectors.Add(mediaDirector);
                }
            }

            _context.SaveChanges();

            return MViewModel.Media!.Id;
        }


        // DELETE: api/Media/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteMedia(int id)
        //{
        //    if (_context.Medias == null)
        //    {
        //        return NotFound();
        //    }
        //    var media = await _context.Medias.FindAsync(id);
        //    if (media == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Medias.Remove(media);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool MediaExists(int id)
        //{
        //    return (_context.Medias?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
