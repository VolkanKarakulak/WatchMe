using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class EpisodesController : ControllerBase
    {
        private readonly WatchMeContext _context;

        public EpisodesController(WatchMeContext context)
        {
            _context = context;
        }

        // GET: api/Episodes
        [HttpGet]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult<List<EpisodeViewModel>> GetEpisodes(int mediaId, byte seasonNumber)
        {
            var episodes = _context.Episodes
                .Where(e => e.MediaId == mediaId && e.SeasonNumber == seasonNumber)
                .OrderBy(e => e.EpisodeNumber)
                .Select(e => new EpisodeViewModel
                {
                    MediaName = e.Media!.Name, 
                    SeasonNumber = e.SeasonNumber,
                    EpisodeNumber = e.EpisodeNumber,
                    Title = e.Title,
                    ReleaseDate = e.ReleaseDate,
                    Description = e.Description,
                    Passive = e.Passive,
                    ViewCount = e.ViewCount
                })
                .ToList();

            return episodes;
        }

        [HttpGet("Watch")]
        [Authorize]
        public void Watch(long id)
        {
            //Find logged in user.
            //Check age
            //If age is less than 18
            //Get media restrictions via episode
            //Check if the user is permitted to view the episode
            UserWatched userWatched = new UserWatched();
            Episode episode = _context.Episodes.Find(id)!;

            try
            {
                userWatched.UserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                userWatched.EpisodeId = id;
                _context.UserWatcheds.Add(userWatched);
                episode.ViewCount++;
                _context.Episodes.Update(episode);
                _context.SaveChanges();
                //İlk izlemede artar
            }
            catch (Exception ex)
            { }
        }

        // PUT: api/Episodes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "ContentAdmin")]
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
        [Authorize(Roles = "ContentAdmin")]
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
        [Authorize(Roles = "ContentAdmin")]
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
