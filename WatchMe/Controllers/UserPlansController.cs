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
        public ActionResult<List<UserPlan>> GetUserPlan(long id)
        {
            List<UserPlan> userPlan = _context.UserPlans.Where(u => u.UserId == id).ToList();

            if (userPlan == null)
            {
                return NotFound("Kayıt Bulunamadı");
            }

            return userPlan;
        }


        // POST: api/UserPlans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostUserPlan(string eMail, short planId)
        {
           
            AppUser appUser = _signInManager.UserManager.FindByEmailAsync(eMail).Result;
            //Get payment for plan.Price;
            //if(payment succesful)
            if (appUser == null)
            {
                return NotFound("Kullanıcı Bulunamadı.");
            }

            UserPlan userPlan = new UserPlan();                                 
            userPlan.PlanId = planId;
            userPlan.UserId = appUser.Id;
            userPlan.StartDate = DateTime.Today;
            userPlan.EndDate = userPlan.StartDate.AddMonths(1);
            _context.UserPlans.Add(userPlan);
            appUser.Passive = false;
            await _context.SaveChangesAsync();

            return Ok("Plan Oluşturuldu");
        }

    }
}
