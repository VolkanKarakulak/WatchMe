using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchMe.Data;
using WatchMe.Models;

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
        public ActionResult<List<Media>> GetMedias()
        {
         
            return _context.Medias.Include(x => x.MediaCategories)!.ThenInclude(x=> x.Category).AsNoTracking().ToList();


        }

        // GET: api/Media/5
        [HttpGet("{id}")]
        public ActionResult<Media> GetMedia(int id)
        {
         
            Media? media = _context.Medias.Find(id);

            MediaCategory? mediacategory = _context.MediaCategories.Where(u => u.MediaId == id).Include(u => u.Category).FirstOrDefault();

            if (media == null)
            {
                return NotFound();
            }

            return media;
        }

        // PUT: api/Media/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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
        public int PostMedia(Media media, int starId, short categoryId, int directorId) // Restriction ekle
        {

            MediaStar mediaStar = new MediaStar();
            MediaCategory mediaCategory = new MediaCategory();
            MediaDirector mediaDirector = new MediaDirector();
             //MediaRestriction mediaRestriction = new MediaRestriction();
             
            _context.Medias.Add(media);
            _context.SaveChanges();

            mediaStar.StarId = starId;
            mediaStar.MediaId = media.Id;
            mediaCategory.CategoryId = categoryId;
            mediaCategory.MediaId = media.Id;
            mediaDirector.DirectorId = directorId;
            mediaDirector.MediaId = media.Id;
            //mediaRestriction.RestrictionId = restrictionId;

            _context.MediaStars.Add(mediaStar);
            _context.MediaCategories.Add(mediaCategory);
            _context.MediaDirectors.Add(mediaDirector);
            _context.SaveChanges();

            return media.Id;
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
