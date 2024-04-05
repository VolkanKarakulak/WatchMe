using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchMe.Data;
using WatchMe.Identity.Data;
using WatchMe.Models;

namespace WatchMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPlansController : ControllerBase
    {
        private readonly WatchMeContext _context;
        private readonly SignInManager<AppUser> _signInManager;

        public UserPlansController(WatchMeContext context, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        // GET: api/UserPlans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserPlan>>> GetUserPlans()
        {
          if (_context.UserPlans == null)
          {
              return NotFound();
          }
            return await _context.UserPlans.ToListAsync();
        }

        // GET: api/UserPlans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserPlan>> GetUserPlan(long id)
        {
          if (_context.UserPlans == null)
          {
              return NotFound();
          }
            var userPlan = await _context.UserPlans.FindAsync(id);

            if (userPlan == null)
            {
                return NotFound("Kayıt Bulunamadı");
            }

            return userPlan;
        }

        // PUT: api/UserPlans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUserPlan(long id, UserPlan userPlan)
        //{
        //    if (id != userPlan.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(userPlan).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
               
        //    }

        //    return NoContent();
        //}

        // POST: api/UserPlans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public void PostUserPlan(string eMail, short planId)
        {
           
            AppUser appuser = _signInManager.UserManager.FindByEmailAsync(eMail).Result;
            //Get payment for plan.Price;
            //if(payment succesful)
            
            UserPlan userPlan = new UserPlan();

            //userPlan.UserId=Find from UserManager with EMail
                
            userPlan.PlanId = planId;
            userPlan.UserId = appuser.Id;
            userPlan.StartDate = DateTime.Today;
            userPlan.EndDate = userPlan.StartDate.AddMonths(1);
            _context.UserPlans.Add(userPlan);
            _context.SaveChanges();
            
        }

        // DELETE: api/UserPlans/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUserPlan(long id)
        //{
        //    if (_context.UserPlans == null)
        //    {
        //        return NotFound();
        //    }
        //    var userPlan = await _context.UserPlans.FindAsync(id);
        //    if (userPlan == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.UserPlans.Remove(userPlan);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool UserPlanExists(long id)
        //{
        //    return (_context.UserPlans?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
