using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using WatchMe.Data;
using WatchMe.Identity.Data;
using WatchMe.Models;
using WatchMe.ViewModels;

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
        public ActionResult<List<UserPlanViewModel>> GetUserPlans()
        {
          List<UserPlan> userPlans = _context.UserPlans
                .Include(up => up.AppUser)
                .Include(up => up.Plan)
                .ToList();

            if (userPlans == null || userPlans.Count == 0)
            {
                return NotFound("User's Plan Not Found");
            }

            List<UserPlanViewModel> userPlanViewModels = userPlans.Select(up => new UserPlanViewModel
            {
                UserName = up.AppUser?.Name,
                PlanName = up.Plan?.Name,
                StartDate = up.StartDate,
                EndDate = up.EndDate
            }).ToList();

            return userPlanViewModels;
        }

        // GET: api/UserPlans/5
        [HttpGet("{id}")]
        public ActionResult<List<UserPlanViewModel>> GetUserPlan(long id)
        {
            AppUser appUser = _signInManager.UserManager.FindByIdAsync(id.ToString()).Result;

            if (appUser == null)
            {
                return NotFound("User Not Found");
            }
            List<UserPlan> userPlans = _context.UserPlans
                .Include(up => up.AppUser)
                .Include(up => up.Plan)
                .Where(up => up.UserId == id)
                .ToList();

            if (userPlans == null || userPlans.Count == 0)
            {
                return NotFound("User's Plan Not Found");
            }

            List<UserPlanViewModel> userPlanViewModels = userPlans.Select(up => new UserPlanViewModel
            {
                UserName = up.AppUser?.Name,
                PlanName = up.Plan?.Name,
                StartDate = up.StartDate,
                EndDate = up.EndDate
            }).ToList();

            return userPlanViewModels;
        }

        // POST: api/UserPlans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult PostUserPlan(string eMail, short planId)
        {
            var paymentSuccessful = true;
           
            AppUser appUser = _signInManager.UserManager.FindByEmailAsync(eMail).Result;

            if (appUser == null)
            {
                return NotFound("User Not Found");
            }

            bool hasActiveSubscription = _context.UserPlans.Any(p => p.UserId == appUser.Id && p.EndDate >= DateTime.Today);
            if (hasActiveSubscription)
            {
                return Ok("You Already Have an Existing Subscription");
            }
            
            //Get payment for plan.Price;
            if(paymentSuccessful)
             {
            UserPlan userPlan = new UserPlan();                                 
            userPlan.PlanId = planId;
            userPlan.UserId = appUser.Id;
            userPlan.StartDate = DateTime.Today;
            userPlan.EndDate = userPlan.StartDate.AddMonths(1);
            _context.UserPlans.Add(userPlan);
            appUser.Passive = false;
            //_signInManager.UserManager.UpdateAsync(appUser).Wait();
            _context.SaveChanges();

            return Ok("Plan Created");
            }
            else
            {
                return BadRequest("Payment Failed");
            }
        }

    }
}
