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
using Microsoft.EntityFrameworkCore;
using WatchMe.Data;
using WatchMe.Identity.Data;
using WatchMe.Models;

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
        public AppUsersController(SignInManager<AppUser> signInManager, WatchMeContext context)
        {
            _signInManager = signInManager;
            _context = context;
        }

        // GET: api/AppUsers
        [HttpGet]
        //[Authorize(Roles = "Administrator")]
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
        //[Authorize]
        public ActionResult<AppUser> GetAppUser(long id)
        {
            AppUser? appUser = null;

            //if (User.IsInRole("Admin") == false)
            //{
            //    if (User.FindFirstValue(ClaimTypes.NameIdentifier) != id.ToString())
            //    {
            //        return Unauthorized();
            //    }
            //}

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
        //[Authorize]
        public  ActionResult  PutAppUser(AppUser appUser)
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
        [HttpPost("LogIn")]
        public ActionResult<List<Media>> LogIn(LogInModel logInModel)
        {


            Microsoft.AspNetCore.Identity.SignInResult signInResult;
            AppUser applicationUser = _signInManager.UserManager.FindByNameAsync(logInModel.UserName).Result; // Result; async FindByNameAsync metodunun çalışmasını senkron bir şekilde çağırarak sonucunu bekler ve sonucu alır.
            List<Media>? medias = null;
            List<UserFavorite> userFavorites;
            IQueryable<Media> mediaQuery;
            IGrouping<short, MediaCategory>? mediaCategories;
            IQueryable<int> userWatcheds;


            if (applicationUser == null)
            {
                return NotFound();
            }
            if (_context.UserPlans.Where(u => u.UserdId == applicationUser.Id && u.EndDate>= DateTime.Today).Any() == false) 
            {
                applicationUser.Passive = true;
                _signInManager.UserManager.UpdateAsync(applicationUser).Wait();

            }
            if (applicationUser.Passive == true)
            {
                return Content("Passive");
            }
            signInResult = _signInManager.PasswordSignInAsync(applicationUser, logInModel.Password, false, false).Result;
            if (signInResult.Succeeded == true)
            {
                //Kullanıcının favori olarak işaretlediği mediaları ve kategorilerini alıyoruz.
                userFavorites = _context.UserFavorites.Where(u => u.UserId == applicationUser.Id).Include(u => u.Media).Include(u => u.Media!.MediaCategories).ToList();
                //userFavorites içindeki media kategorilerini ayıklıyoruz (SelectMany)
                //Bunları kategori id'lerine göre grupluyoruz (GroupBy)
                //Her grupta kaç adet item olduğuna bakıp (m.Count())
                //Çoktan aza doğru sıralıyoruz (OrderByDescending)
                //En üstteki, yani en çok item'a sahip grubu seçiyoruz (FirstOrDefault)
                mediaCategories = userFavorites.SelectMany(u => u.Media!.MediaCategories!).GroupBy(m => m.CategoryId).OrderByDescending(m => m.Count()).FirstOrDefault();
                if (mediaCategories != null)
                {
                    //Kullabıcının izlediği episode'lardan media'ya ulaşıp, sadece media id'lerini alıyoruz (Select)
                    //Tekrar eden media id'leri eliyoruz (Distinct)
                    userWatcheds = _context.UserWatcheds.Where(u => u.UserId == applicationUser.Id).Include(u => u.Episode).Select(u => u.Episode!.MediaId).Distinct();
                    //Öneri yapmak için mediaCategories.Key'i yani kullanıcının en çok favorilediği kategori id'sini kullanıyoruz
                    //Media listesini çekerken sadece bu kategoriye ait mediaların MediaCategories listesini dolduruyoruz (Include(m => m.MediaCategories!.Where(mc => mc.CategoryId == mediaCategories.Key)))
                    //Diğer mediaların MediaCategories listeleri boş kalıyor
                    //Sonrasında MediaCategories'i boş olmayan media'ları filtreliyoruz (m.MediaCategories!.Count > 0)
                    //Ayrıca bu kategoriye giren fakat kullanıcının izlemiş olduklarını da dışarıda bırakıyoruz (userWatcheds.Contains(m.Id) == false)
                    mediaQuery = _context.Medias.Include(m => m.MediaCategories!.Where(mc => mc.CategoryId == mediaCategories.Key)).Where(m => m.MediaCategories!.Count > 0 && userWatcheds.Contains(m.Id) == false);
                    if (applicationUser.Restriction != null)
                    {
                        //TO DO
                        //Son olarak, kullanıcı bir restrictiona sahipse seçilen media içerisinden bunları da çıkarmamız gerekiyor.
                        mediaQuery = mediaQuery.Include(m => m.MediaRestrictions!.Where(r => r.RestrictionId <= applicationUser.Restriction));
                    }
                    medias = mediaQuery.ToList();
                }
                //Populate medias
            }
            return medias;
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