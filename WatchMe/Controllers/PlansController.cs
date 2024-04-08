using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class PlansController : ControllerBase
    {
        private readonly WatchMeContext _context;

        public PlansController(WatchMeContext context)
        {
            _context = context;
        }

        // GET: api/Plans
        [HttpGet]
        public ActionResult<List<PlanViewModel>> GetPlans()
        {
            if (_context.Plans == null)
            {
                return NotFound();
            }

            var plans = _context.Plans.Select(p => new PlanViewModel
            {
                Name = p.Name,
                Price = p.Price,
                Resolution = p.Resolution
            }).ToList();

            return Ok(plans);
        }


        // GET: api/Plans/5
        [HttpGet("{id}")]
        public ActionResult<PlanViewModel> GetPlan(short id)
        {
            Plan? plan = _context.Plans.Find(id);

            if (plan == null)
            {
                return NotFound();
            }

            var planViewModel = new PlanViewModel
            {
                Name = plan.Name,
                Price = plan.Price,
                Resolution = plan.Resolution
            };

            return Ok(planViewModel);
        }

        // PUT: api/Plans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutPlan(short id, Plan plan)
        {
            if (id != plan.Id)
            {
                return BadRequest();
            }

            _context.Entry(plan).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanExists(id))
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

        // POST: api/Plans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Plan> PostPlan(Plan plan)
        {
          if (_context.Plans == null)
          {
              return Problem("Entity set 'WatchMeContext.Plans'  is null.");
          }
            _context.Plans.Add(plan);
            _context.SaveChanges();

            return CreatedAtAction("GetPlan", new { id = plan.Id }, plan);
        }

        // DELETE: api/Plans/5
        [HttpDelete("{id}")]
        public ActionResult DeletePlan(short id)
        {
            if (_context.Plans == null)
            {
                return NotFound();
            }
            var plan = _context.Plans.Find(id);
            if (plan == null)
            {
                return NotFound();
            }

            _context.Plans.Remove(plan);
            _context.SaveChanges();

            return NoContent();
        }

        private bool PlanExists(short id)
        {
            return (_context.Plans?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
