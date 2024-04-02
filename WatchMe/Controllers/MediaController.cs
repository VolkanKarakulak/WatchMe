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
         
            return _context.Medias.AsNoTracking().ToList();
        }

        // GET: api/Media/5
        [HttpGet("{id}")]
        public ActionResult<Media> GetMedia(int id)
        {
         
            Media? media = _context.Medias.Find(id);

            if (media == null)
            {
                return NotFound();
            }

            return media;
        }

        // PUT: api/Media/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public void PutMedia(Media media)
        {
            _context.Medias.Update(media);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        // POST: api/Media
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public int PostMedia(Media media, short categoryId)
        {
             MediaCategory mediaCategory = new MediaCategory();

            _context.Medias.Add(media);
            _context.SaveChanges();
            
            mediaCategory.CategoryId = categoryId;
            mediaCategory.MediaId = media.Id;

            _context.MediaCategories.Add(mediaCategory);
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
