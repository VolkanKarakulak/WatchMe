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
    //[Authorize(Roles = "ContentAdmin")]
    public class RestrictionsController : ControllerBase
    {
        private readonly WatchMeContext _context;

        public RestrictionsController(WatchMeContext context)
        {
            _context = context;
        }

        // GET: api/Restrictions
        [HttpGet]
        public ActionResult<IEnumerable<Restriction>> GetRestrictions()
        {
          if (_context.Restrictions == null)
          {
              return NotFound();
          }
            return _context.Restrictions.ToList();
        }

        // GET: api/Restrictions/5
        [HttpGet("{id}")]
        public ActionResult<Restriction> GetRestriction(byte id)
        {
            if (_context.Restrictions == null)
            {
                return NotFound();
            }
            Restriction? restriction = _context.Restrictions.Find(id);

            if (restriction == null)
            {
                return NotFound();
            }

            return restriction;
        }

        // PUT: api/Restrictions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutRestriction(byte id, Restriction restriction)
        {
            if (id != restriction.Id)
            {
                return BadRequest();
            }

            _context.Entry(restriction).State = EntityState.Modified;

            try
            {
                 _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestrictionExists(id))
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

        // POST: api/Restrictions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Restriction> PostRestriction(Restriction restriction)
        {
          if (_context.Restrictions == null)
          {
              return Problem("Entity set 'WatchMeContext.Restrictions'  is null.");
          }
            _context.Restrictions.Add(restriction);
            try
            {
                 _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (RestrictionExists(restriction.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRestriction", new { id = restriction.Id }, restriction);
        }

        // DELETE: api/Restrictions/5
        [HttpDelete("{id}")]
        public IActionResult DeleteRestriction(byte id)
        {
            if (_context.Restrictions == null)
            {
                return NotFound();
            }
            var restriction = _context.Restrictions.Find(id);
            if (restriction == null)
            {
                return NotFound();
            }

            _context.Restrictions.Remove(restriction);
             _context.SaveChanges();

            return NoContent();
        }
        [HttpGet("restriction_id")]
        [Authorize]
        public ActionResult<List<MediaRestriction>> Restriction_Medias(byte restrictionId)
        {
            List<MediaRestriction> mediaRestriction = _context.MediaRestrictions.Where(m => m.RestrictionId == restrictionId).ToList();
            if (mediaRestriction == null)
            {
                return NotFound();
            }
            return mediaRestriction;
        }
        private bool RestrictionExists(byte id)
        {
            return (_context.Restrictions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
