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
    public class StarsController : ControllerBase
    {
        private readonly WatchMeContext _context;

        public StarsController(WatchMeContext context)
        {
            _context = context;
        }

        // GET: api/Stars
        [HttpGet]
        public ActionResult<List<Star>> GetStars()
        {
          if (_context.Stars == null)
          {
              return NotFound();
          }
            return _context.Stars.ToList();
        }

        // GET: api/Stars/5
        [HttpGet("{id}")]
        public ActionResult<Star> GetStar(int id)
        {
          
            Star? star = _context.Stars.Find(id);

            if (star == null)
            {
                return NotFound();
            }

            return star;
        }

        [HttpGet("mediastars/{starId}")]
        //[Authorize]
        public ActionResult<List<MediaStarViewModel>> GetMediaStar(int starId)
        {
            List<MediaStarViewModel> mediaStars = _context.MediaStars
            .Where(ms => ms.StarId == starId)
            .Select(ms => new MediaStarViewModel
            {
            MediaName = ms.Media!.Name,
            StarName = ms.Star!.Name
            })
            .ToList();


            if (mediaStars == null)
            {
                return NotFound();
            }

            return mediaStars;
        }


        // PUT: api/Stars/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public ActionResult PutStar(int id, Star star)
        {
            if (id != star.Id)
            {
                return BadRequest();
            }

            _context.Entry(star).State = EntityState.Modified;

            try
            {
                 _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StarExists(id))
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

        // POST: api/Stars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Star> PostStar(Star star)
        {
          if (_context.Stars == null)
          {
              return Problem("Entity set 'WatchMeContext.Stars'  is null.");
          }
            _context.Stars.Add(star);
            _context.SaveChanges();

            return CreatedAtAction("GetStar", new { id = star.Id }, star);
        }

        // DELETE: api/Stars/5
        [HttpDelete("{id}")]
        public ActionResult DeleteStar(int id)
        {
            if (_context.Stars == null)
            {
                return NotFound();
            }
            Star? star =  _context.Stars.Find(id);
            if (star == null)
            {
                return NotFound();
            }

            _context.Stars.Remove(star);
            _context.SaveChanges();

            return NoContent();
        }

        private bool StarExists(int id)
        {
            return (_context.Stars?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
