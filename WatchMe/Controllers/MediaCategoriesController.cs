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
    public class MediaCategoriesController : ControllerBase
    {
        private readonly WatchMeContext _context;

        public MediaCategoriesController(WatchMeContext context)
        {
            _context = context;
        }

        // GET: api/MediaCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MediaCategory>>> GetMediaCategories()
        {
          if (_context.MediaCategories == null)
          {
              return NotFound();
          }
            return await _context.MediaCategories.ToListAsync();
        }

        // GET: api/MediaCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MediaCategory>> GetMediaCategory(int id)
        {
          if (_context.MediaCategories == null)
          {
              return NotFound();
          }
            var mediaCategory = await _context.MediaCategories.FindAsync(id);

            if (mediaCategory == null)
            {
                return NotFound();
            }

            return mediaCategory;
        }

        // PUT: api/MediaCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMediaCategory(int id, MediaCategory mediaCategory)
        {
            if (id != mediaCategory.MediaId)
            {
                return BadRequest();
            }

            _context.Entry(mediaCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MediaCategoryExists(id))
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

        // POST: api/MediaCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MediaCategory>> PostMediaCategory(MediaCategory mediaCategory)
        {
          if (_context.MediaCategories == null)
          {
              return Problem("Entity set 'WatchMeContext.MediaCategories'  is null.");
          }
            _context.MediaCategories.Add(mediaCategory);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MediaCategoryExists(mediaCategory.MediaId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMediaCategory", new { id = mediaCategory.MediaId }, mediaCategory);
        }

        // DELETE: api/MediaCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMediaCategory(int id)
        {
            if (_context.MediaCategories == null)
            {
                return NotFound();
            }
            var mediaCategory = await _context.MediaCategories.FindAsync(id);
            if (mediaCategory == null)
            {
                return NotFound();
            }

            _context.MediaCategories.Remove(mediaCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MediaCategoryExists(int id)
        {
            return (_context.MediaCategories?.Any(e => e.MediaId == id)).GetValueOrDefault();
        }
    }
}
