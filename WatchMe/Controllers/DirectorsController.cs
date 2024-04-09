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
    public class DirectorsController : ControllerBase
    {
        private readonly WatchMeContext _context;

        public DirectorsController(WatchMeContext context)
        {
            _context = context;
        }

        // GET: api/Directors
        [HttpGet]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult<List<Director>> GetDirectors()
        {
          if (_context.Directors == null)
          {
              return NotFound();
          }
            return  _context.Directors.ToList();
        }

        // GET: api/Directors/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<List<MediaInfoViewModel>> GetDirectorMedia(int id)
        {
            var directorMedia = _context.MediaDirectors
                .Where(md => md.DirectorId == id)
                .Include(md => md.Media)            
                .ToList();

            if (directorMedia == null)
            {
                return NotFound();
            }

            var mediaInfoList = directorMedia.Select(md => new MediaInfoViewModel
            {
                MediaName = md.Media?.Name,               
                IsPassive = md.Media?.Passive
            }).ToList();

            return mediaInfoList;
        }


        // PUT: api/Directors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize(Roles = "ContentAdmin")]
        public void PutDirector(Director director)
        {
            //_context.Entry(director).State = EntityState.Modified;
            _context.Directors.Update(director);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                
            }
        }

 
        [HttpPost]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult<Director> PostDirector(Director director)
        {
          if (_context.Directors == null)
          {
              return Problem("Entity set 'WatchMeContext.Directors'  is null.");
          }
            _context.Directors.Add(director);
            _context.SaveChanges();

            return Ok();
        }

        // DELETE: api/Directors/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult DeleteDirector(int id)
        {
            if (_context.Directors == null)
            {
                return NotFound();
            }
            Director? director =  _context.Directors.Find(id);
            if (director == null)
            {
                return NotFound();
            }

            _context.Directors.Remove(director);
             _context.SaveChanges();

            return NoContent();
        }

        private bool DirectorExists(int id)
        {
            return (_context.Directors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
