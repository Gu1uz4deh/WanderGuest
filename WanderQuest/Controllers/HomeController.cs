using Data.DAL;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WanderQuest.ViewModel;

namespace WanderQuest.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM();
            homeVM.Products = await _context.Products
                .Include(n => n.Category)
                .Include(n => n.ProductImages)
                .ThenInclude(n => n.Image)
                .Where(n => !n.IsDeleted)
                .OrderByDescending(n => n.CreatedDate)
                .Take(5)
                .ToListAsync();
            homeVM.Sliders = await _context.Sliders.Where(n => !n.IsDeleted).OrderByDescending(n => n.CreatedDate).ToListAsync();
            homeVM.Categories = await _context.Categories.Where(n => !n.IsDeleted).OrderByDescending(n => n.CreatedDate).ToListAsync();

            return View(homeVM);
        }
    }
}
