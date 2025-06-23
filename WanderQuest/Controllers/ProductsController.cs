using System;
using Data.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WanderQuest.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Where(n => !n.IsDeleted).Include(n => n.Category).Take(4).ToListAsync();
            return View(products);
        }

        [Route("{controller}/loadmore/{skip}")]
        public async Task<IActionResult> LoadMore(int skip)
        {
            var products = await _context.Products.Where(n => !n.IsDeleted)
                                                    .Include(n => n.Category)
                                                    .Skip(skip)
                                                    .Take(4)
                                                    .ToListAsync();
            return PartialView("_ProductPartial", products);
        }
    }
}
