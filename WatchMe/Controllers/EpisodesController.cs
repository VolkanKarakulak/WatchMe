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

namespace WatchMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpisodesController : ControllerBase
    {
        private readonly WatchMeContext _context;

        public EpisodesController(WatchMeContext context)
        {
            _context = context;
        }

        // GET: api/Episodes
        [HttpGet]
        public ActionResult<List<Episode>> GetEpisodes(int mediaId, byte seasonNumber)
        {
          
            return _context.Episodes.Where(e => e.MediaId == mediaId && e.SeasonNumber == seasonNumber).OrderBy(e => e.EpisodeNumber).ToList();
        }

        // GET: api/Episodes/5
        [HttpGet("{id}")]
        //[Authorize]
        public ActionResult<Episode> GetEpisode(long id)
        {
            Episode? episode = _context.Episodes.Find(id);

            if (episode == null)
            {
                return NotFound();
            }

            return episode;
        }

        // PUT: api/Episodes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public ActionResult PutEpisode(long id, Episode episode)
        {
            if (id != episode.Id)
            {
                return BadRequest();
            }

            _context.Episodes.Update(episode);
            //_context.Entry(episode).State = EntityState.Modified

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                
            }

            return NoContent();
        }

        // POST: api/Episodes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Episode> PostEpisode(Episode episode)
        {
          if (_context.Episodes == null)
          {
              return Problem("Entity set 'WatchMeContext.Episodes'  is null.");
          }
            _context.Episodes.Add(episode);
            _context.SaveChanges();

            return CreatedAtAction("GetEpisode", new { id = episode.Id }, episode);
        }

        // DELETE: api/Episodes/5
        [HttpDelete("{id}")]
        public ActionResult DeleteEpisode(long id)
        {
            var episode = _context.Episodes.Find(id);

            if(episode != null)
            {
                episode.Passive = true;
                _context.SaveChanges();
            }

            return Ok();
        }

        private bool EpisodeExists(long id)
        {
            return (_context.Episodes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
