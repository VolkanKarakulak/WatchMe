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
    public class CategoriesController : ControllerBase
    {
        private readonly WatchMeContext _context;

        public CategoriesController(WatchMeContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public ActionResult<List<Category>> GetCategories()
        {

            return _context.Categories.AsNoTracking().ToList(); // asnotracking hafızada tutmaz
            //return _context.Categories.ToList();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
       //[Authorize]
        public ActionResult<List<MediaCategory>> GetCategory(short id)
        {
         
            Category? category = _context.Categories.Find(id);
            List<MediaCategory> mediaCategory = _context.MediaCategories.Where(x => x.CategoryId == id).Include(x => x.Media).ToList();

           

            return mediaCategory;
        }

        // PUT: api/Categories/5  // Put için id'li yöntem.
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        ////[Authorize(Roles = "ContentAdmin")]
        //public async Task<ActionResult> PutCategory(short? id, Category category)
        //{
        //    if (id == null || _context.Categories == null)
        //    {
        //        return NotFound();
        //    }
        //    var existcategory =  _context.Categories.Find(id);

        //    existcategory.Name = category.Name;


        //    _context.Categories.Update(existcategory);
        //    //_context.Entry(existcategory).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return NoContent();
        //}

        //[HttpPut("{id}")] // Put için id'Li diğer yöntem
        ////[Authorize(Roles = "ContentAdmin")]
        //public ActionResult PutCategory(short? id, Category category)
        //{
        //    if (id == null || _context.Categories == null)
        //    {
        //        return NotFound();
        //    }
        //    var existcategory = _context.Categories.Find(id);

        //    existcategory!.Name = category.Name;


        //    _context.Categories.Update(existcategory);
        //    //_context.Entry(existcategory).State = EntityState.Modified;

        //    try
        //    {
        //         _context.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return NoContent();
        //}


        [HttpPut]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult PutCategory(Category category)
        {

            _context.Categories.Update(category);
            //_context.Entry(existcategory).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            return NoContent();
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[Authorize(Roles = "ContentAdmin")]
        public short PostCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();

            return category.Id;
        }

        private bool CategoryExists(short id)
        {
            return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
