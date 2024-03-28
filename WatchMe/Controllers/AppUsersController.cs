using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchMe.Data;
using WatchMe.Identity.Data;

namespace WatchMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUsersController : ControllerBase
    {
        //private readonly WatchMeContext _context;
        private readonly SignInManager<AppUser> _signInManager;

        public AppUsersController(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }

        // GET: api/AppUsers
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult<List<AppUser>> GetUsers(bool includePassive = true)
        {
            IQueryable<AppUser> users = _signInManager.UserManager.Users;

            if (includePassive == false)
            {
                users = users.Where(u => u.Passive == false);
            }
            return users.AsNoTracking().ToList();
        }

        // GET: api/AppUsers/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<AppUser> GetAppUser(long id)
        {
            AppUser? appUser = null;

            if (User.IsInRole("Admin") == false)
            {
                if (User.FindFirstValue(ClaimTypes.NameIdentifier) != id.ToString())
                {
                    return Unauthorized();
                }
            }

            appUser = _signInManager.UserManager.Users.Where(u => u.Id == id).FirstOrDefault();
            

            if (appUser == null)
            {
                 return NotFound();
            }

            return appUser;
        }

        // PUT: api/AppUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public  ActionResult  PutAppUser(AppUser appUser)
        {
            AppUser? user = null;

            if (User.IsInRole("CustomerRepresantative") == false)
            {
                if (User.FindFirstValue(ClaimTypes.NameIdentifier) != appUser.Id.ToString())
                {
                    return Unauthorized();
                }
            }

            user = _signInManager.UserManager.Users.Where(u => u.Id == appUser.Id).AsNoTracking().FirstOrDefault(); // asnotricking olmaz burda,oku ve unut diyemeyiz

           _signInManager.UserManager.UpdateAsync(appUser);

            if (user == null) 
            {
                return NotFound();
            }

            user.PhoneNumber = appUser.PhoneNumber;
            user.UserName = appUser.UserName;  
            user.BirthDate = appUser.BirthDate;
            user.Email = appUser.Email;
            user.Name = appUser.Name;
            _signInManager.UserManager.UpdateAsync(user).Wait();
            return Ok();
           
        }

        // POST: api/AppUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<string> PostAppUser(AppUser appUser)
        {

            if (User.Identity!.IsAuthenticated == true)
            {
                return BadRequest();
            }

            IdentityResult identityResult = _signInManager.UserManager.CreateAsync(appUser, appUser.PassWord).Result;

            if (identityResult != IdentityResult.Success)
            {
                return identityResult.Errors.FirstOrDefault()!.Description;
            }

           return Ok();

        }

        // DELETE: api/AppUsers/5
        [HttpDelete("{id}")]
        public ActionResult DeleteSoftITOFlixUser(long id)
        {
            AppUser? user = null;

            if (User.IsInRole("CustomerRepresentative") == false)
            {
                if (User.FindFirstValue(ClaimTypes.NameIdentifier) != id.ToString())
                {
                    return Unauthorized();
                }
            }
            user = _signInManager.UserManager.Users.Where(u => u.Id == id).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }
            user.Passive = true;
            _signInManager.UserManager.UpdateAsync(user).Wait();
            return Ok();
        }
    }
}
