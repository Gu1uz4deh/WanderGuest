using System;
using Data.DAL;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WanderQuest.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.Where(n => !n.IsDeleted).ToListAsync(); 
            return View(categories);
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await _context.Categories.Where(n => !n.IsDeleted && n.Id == id)
                .FirstOrDefaultAsync();
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        public async Task<IActionResult> Update(int id)
        {
            var category = await _context.Categories.Where(n => !n.IsDeleted && n.Id == id)
                .FirstOrDefaultAsync();
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, string categoryName)
        {
            var category = await _context.Categories.Where(n => !n.IsDeleted && n.Id == id)
                .FirstOrDefaultAsync();
            if (category == null || string.IsNullOrEmpty(categoryName.Trim()))
            {
                return NotFound();
            }
            category.Name = categoryName.Trim();
            category.UpdatedDate = DateTime.Now;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync(); 

            return RedirectToAction(actionName:nameof(Details), controllerName:nameof(Category) , routeValues: new { id });
        }

        public IActionResult Create()
        {
            return View();
        }
        #region OldCreate
        //[HttpPost]
        //public async Task<IActionResult> Create(string categoryName)
        //{
        //    var category = new Category();
        //    if (string.IsNullOrEmpty(categoryName.Trim()))
        //    {
        //        return NotFound();
        //    }
        //    category.Name = categoryName.Trim();
        //    category.CreatedDate = DateTime.Now;
        //    category.UpdatedDate = null;

        //    _context.Categories.Add(category);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(actionName:nameof(Index), controllerName:nameof(Category));
        //}
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (await isCategoryExist(category.Name))
            {
                return Content("Name is not available");
            }
            category.CreatedDate = DateTime.Now;
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(actionName:nameof(Index), controllerName:nameof(Category));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.Where(n => !n.IsDeleted && n.Id == id)
                .FirstOrDefaultAsync();

            if(category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Category category)
        {
            var dbCategory = await _context.Categories.Where(n => !n.IsDeleted && n.Id == category.Id)
                .FirstOrDefaultAsync();

            if(dbCategory.Name.Trim().ToLower() != category.Name.Trim().ToLower())
            {
                return Content("Cannot Delete");
            }

            if (dbCategory == null)
            {
                return NotFound();
            }
            dbCategory.IsDeleted = true;

            _context.Categories.Update(dbCategory);
            await _context.SaveChangesAsync();

            return RedirectToAction(actionName: nameof(Index), controllerName: nameof(Category));
        }

        private async Task<bool> isCategoryExist(string name)
        {
            var isExist = await _context.Categories.AnyAsync(n => n.Name.Trim().ToLower() == name.Trim().ToLower());
            return isExist;
        }
    }
}
 