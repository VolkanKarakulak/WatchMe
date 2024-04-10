using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using WatchMe.Data;
using WatchMe.Identity.Data;
using WatchMe.Models;
using WatchMe.ViewModels;


namespace WatchMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUsersController : ControllerBase
    {
        //private readonly WatchMeContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly WatchMeContext _context;

        public struct LogInModel
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
        public struct MediaModel
        {
            public Media media { get; set; }
            public int ViewCount { get; set; }
        }
        public struct ChangePasswordModel
        {
            public string UserName { get; set; }
            public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
        }
        public AppUsersController(SignInManager<AppUser> signInManager, WatchMeContext context)
        {
            _signInManager = signInManager;
            _context = context;
        }

        // GET: api/AppUsers
        [HttpGet]
        [Authorize(Roles = "Admin")]
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
        public ActionResult  PutAppUser(AppUser appUser)
        {
            AppUser? user = null;

            //if (User.IsInRole("CustomerRepresantative") == false)
            //{
            //    if (User.FindFirstValue(ClaimTypes.NameIdentifier) != appUser.Id.ToString())
            //    {
            //        return Unauthorized();
            //    }
            //}

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

           return Ok("User Account Created");

        }

        [HttpPut("ActivateUser")]
        [Authorize(Roles = "Admin")]
        public ActionResult ActivateUser(long id)
        {
            AppUser? appUser = _signInManager.UserManager.FindByIdAsync(id.ToString()).Result;

            if (appUser == null)
            {
                return NotFound();
            }
            appUser.Passive = false;

            _signInManager.UserManager.UpdateAsync(appUser).Wait();
            return Ok("User Aktivated");
        }


        //[HttpPost("SetPassword")]
        //[Authorize]
        //public ActionResult<string> SetPassword(LogInModel logInModel)
        //{
        //    AppUser? appUser = _signInManager.UserManager.FindByNameAsync(logInModel.UserName).Result;

        //    if (appUser == null)
        //    {
        //        return Problem("User Not Found");
        //    }
        //    try
        //    {
        //        _signInManager.UserManager.RemovePasswordAsync(appUser).Wait();
        //        _signInManager.UserManager.AddPasswordAsync(appUser, logInModel.Password).Wait();
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest("Password Set Failed");
        //    }
        //    return Ok("Password Set Successfully");
        //}

        [HttpPost("ChangePassword")]
        [Authorize]
        public ActionResult<string> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            AppUser? appUser = _signInManager.UserManager.FindByNameAsync(changePasswordModel.UserName).Result;
            if (appUser == null)
            {
                return NotFound("User Not Found");
            }
            IdentityResult changePasswordResult = _signInManager.UserManager.ChangePasswordAsync(appUser, changePasswordModel.CurrentPassword, changePasswordModel.NewPassword).Result;
            if(!changePasswordResult.Succeeded)
            {
                return BadRequest("Password change failed");
            }
            return Ok("Password Changed Succesfully");
        }

        // DELETE: api/AppUsers/5
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult<string> DeleteAppUser(long id)
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
            return Ok("User Deactivated");
        }

        [HttpPost("LogIn")]
        public ActionResult<List<Media>> LogIn(LogInModel logInModel)
        {
            Microsoft.AspNetCore.Identity.SignInResult signInResult;
            AppUser appUser = _signInManager.UserManager.FindByNameAsync(logInModel.UserName).Result; // Result; async FindByNameAsync metodunun çalışmasını senkron bir şekilde çağırarak sonucunu bekler ve sonucu alır.

            List<Media>? topMedias = null;
            List<UserFavorite> userFavorites;
            IQueryable<Media> mediaQuery;
            IGrouping<short, MediaCategory>? mediaCategories;
            IQueryable<int> userWatcheds;
            List<MediaModel> mediaModels = new List<MediaModel>();
           //List<string> media = _context.Medias.Take(3).Select(m => m.Name).ToList(); // sadece 2 adet media listenelir, kolaylık açısından 2 adet seçilmiştir isteğe göre artırılabilir
         

            if (appUser == null)
            {
                return NotFound();
            }

            if (_signInManager.UserManager.GetRolesAsync(appUser).Result.Any(r => r == "Admin" || r == "ContentAdmin" || r == "CustomerRepresentative"))
            {
                signInResult = _signInManager.PasswordSignInAsync(appUser, logInModel.Password, false, false).Result;

                if (signInResult.Succeeded)
                {
                    return Ok("Admin LoggedIn");
                }
            }
            //if (_context.UserFavorites.Where(u => u.UserId == appUser.Id).Any() == false && _context.UserPlans.Where(k => k.UserId == appUser.Id).Any() == false) // eğer giriş yapan kullanıcının userfavoritesi null ise List mediaları görsün
            //{
            //    return Ok(media);
            //}

            //if (_context.UserPlans.Where(u => u.UserId == appUser.Id && u.EndDate >= DateTime.Today).Any() == false)
            //{
            //    appUser.Passive = true;
            //    _signInManager.UserManager.UpdateAsync(appUser).Wait();
            //    return Ok(new { Message = "User LoggedIn", Önerilenler = media });

            //}
            //if (appUser.Passive == true)
            //{
            //    return Content("Passive");
            //}
            signInResult = _signInManager.PasswordSignInAsync(appUser, logInModel.Password, false, false).Result;

            if (signInResult.Succeeded == false)
            {
                return BadRequest("Login failed. Please Check Your Credentials");
            }

            if (signInResult.Succeeded == true)
            {
                userFavorites = _context.UserFavorites.Where(u => u.UserId == appUser.Id).Include(u => u.Media).Include(u => u.Media!.MediaCategories).ToList();
                
                mediaCategories = userFavorites.SelectMany(u => u.Media!.MediaCategories!).GroupBy(m => m.CategoryId).OrderByDescending(m => m.Count()).FirstOrDefault();
                if (mediaCategories != null)
                {
                    userWatcheds = _context.UserWatcheds.Where(u => u.UserId == appUser.Id).Include(u => u.Episode).Select(u => u.Episode!.MediaId).Distinct();
                    
                    mediaQuery = _context.Medias.Include(m => m.MediaCategories!.Where(mc => mc.CategoryId == mediaCategories.Key)).Where(m => m.MediaCategories!.Count > 0 && userWatcheds.Contains(m.Id) == false);
                    if (appUser.Restriction != null)
                    {
                     
                        mediaQuery = mediaQuery.Include(m => m.MediaRestrictions!.Where(r => r.RestrictionId <= appUser.Restriction));
                    }
                    else
                    {
                        mediaQuery = _context.Medias.Include(m => m.MediaRestrictions!.Where(r => r.RestrictionId <= appUser.Restriction));
                    }

                    var mediamodels = mediaQuery.Select(med => new MediaModel
                    {
                        media = med,

                        ViewCount = _context.UserWatcheds.Include(uw => uw.Episode!.Media).Count(uw => uw.Episode!.MediaId == med.Id)

                    }
                    ).OrderByDescending(m => m.ViewCount).Take(2).ToList();
                    
                    // Şimdi, Media nesnelerini içeren bir koleksiyon oluşturabiliriz
                    topMedias = mediamodels.Select(mvm => mvm.media).ToList();
                }
               
            }
            return topMedias!;
        }

        [HttpPost("Logout")]
        //[Authorize]
        public ActionResult LogOut()
        {
             _signInManager.SignOutAsync().Wait();

            return Ok("Successful Logout");
        }

    }
}


// Önerilerin Uzun Yöntemi
// userFavorites = _context.UserFavorites.Where(u => u.UserId == applicationUser.Id);
//userFavorites = userFavorites.Include(u => u.Media);
//userFavorites = userFavorites.Include(u => u.Media!.MediaCategories);
//mediaCategories = userFavorites.ToList().SelectMany(u => u.Media!.MediaCategories!).GroupBy(m => m.CategoryId).OrderByDescending(m => m.Count()).FirstOrDefault();

//if (mediaCategories != null)
//{
//    userWatcheds = _context.UserWatcheds.Where(u => u.UserId == applicationUser.Id).Include(u => u.Episode).Select(u => u.Episode!.MediaId).Distinct();
//    mediaQuery = _context.Medias.Include(m => m.MediaCategories!.Where(mc => mc.CategoryId == mediaCategories.Key)).Where(m => userWatcheds.Contains(m.Id) == false);
//    if (applicationUser.Restriction != null)
//    {
//        mediaQuery = mediaQuery.Include(m => m.MediaRestrictions!.Where(r => r.RestrictionId <= applicationUser.Restriction));
//    }

//    medias = mediaQuery.ToList();
//}
//            }
//            return medias;