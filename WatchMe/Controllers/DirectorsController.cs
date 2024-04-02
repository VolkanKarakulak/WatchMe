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
    public class DirectorsController : ControllerBase
    {
        private readonly WatchMeContext _context;

        public DirectorsController(WatchMeContext context)
        {
            _context = context;
        }

        // GET: api/Directors
        [HttpGet]
        public ActionResult<IEnumerable<Director>> GetDirectors()
        {
          if (_context.Directors == null)
          {
              return NotFound();
          }
            return  _context.Directors.ToList();
        }

        // GET: api/Directors/5
        [HttpGet("{id}")]
        public ActionResult<Director> GetDirector(int id)
        {
          if (_context.Directors == null)
          {
              return NotFound();
          }
            var director = _context.Directors.Find(id);

            if (director == null)
            {
                return NotFound();
            }

            return director;
        }

        // PUT: api/Directors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
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

        // POST: api/Directors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
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
        public async Task<IActionResult> DeleteDirector(int id)
        {
            if (_context.Directors == null)
            {
                return NotFound();
            }
            var director = await _context.Directors.FindAsync(id);
            if (director == null)
            {
                return NotFound();
            }

            _context.Directors.Remove(director);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DirectorExists(int id)
        {
            return (_context.Directors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
