using System;
using Data.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WanderQuest.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [Area("Admin")]
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.Where(n => !n.IsDeleted).ToListAsync(); 
            return View(categories);
        }
    }
}
