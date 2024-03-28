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
    public class EpisodesController : ControllerBase
    {
        private readonly WatchMeContext _context;

        public EpisodesController(WatchMeContext context)
        {
            _context = context;
        }

        // GET: api/Episodes
        [HttpGet]
        public ActionResult<List<Episode>> GetEpisodes()
        {
        
            return  _context.Episodes.AsNoTracking().ToList();
        }

        // GET: api/Episodes/5
        [HttpGet("{id}")]
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
        public  void PutEpisode(Episode episode)
        {

            _context.Episodes.Update(episode);

            try
            {
                 _context.SaveChanges();
            }
            catch (Exception ex) 
            {
                
            }
        }

        // POST: api/Episodes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public long PostEpisode(Episode episode)
        {
         
            _context.Episodes.Add(episode);
             _context.SaveChanges();

            return episode.Id;
        }

        // DELETE: api/Episodes/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteEpisode(long id)
        //{
        //    if (_context.Episodes == null)
        //    {
        //        return NotFound();
        //    }
        //    var episode = await _context.Episodes.FindAsync(id);
        //    if (episode == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Episodes.Remove(episode);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool EpisodeExists(long id)
        //{
        //    return (_context.Episodes?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
