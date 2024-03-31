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

            //_context.Episodes.Update(episode);
            //_context.Entry(episode).State = EntityState.Modified

            var existingEpisode = _context.Episodes.Find(id);

            if (existingEpisode == null)
            {
                return NotFound();
            }

            existingEpisode.EpisodeNumber = episode.EpisodeNumber;
            existingEpisode.SeasonNumber = episode.SeasonNumber;
            existingEpisode.Title = episode.Title;
            existingEpisode.ReleaseDate = episode.ReleaseDate;
            existingEpisode.Description =episode.Description;
            existingEpisode.Duration = episode.Duration;
            existingEpisode.Passive = episode.Passive;
            existingEpisode.ViewCount = episode.ViewCount;

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
        public async Task<ActionResult<Episode>> PostEpisode(Episode episode)
        {
          if (_context.Episodes == null)
          {
              return Problem("Entity set 'WatchMeContext.Episodes'  is null.");
          }
            _context.Episodes.Add(episode);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEpisode", new { id = episode.Id }, episode);
        }

        // DELETE: api/Episodes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEpisode(long id)
        {
            if (_context.Episodes == null)
            {
                return NotFound();
            }
            var episode = await _context.Episodes.FindAsync(id);
            if (episode == null)
            {
                return NotFound();
            }

            _context.Episodes.Remove(episode);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EpisodeExists(long id)
        {
            return (_context.Episodes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
