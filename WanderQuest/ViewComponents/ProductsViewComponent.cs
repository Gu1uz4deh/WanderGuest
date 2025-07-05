using System;
using Data.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WanderQuest.ViewComponents
{
    public class ProductsViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public ProductsViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int skip = 0, int take = 4)
        {
            var products = await _context.Products.Where(n => !n.IsDeleted)
                                                  .Include(n => n.Category)
                                                  .Include(n => n.ProductImages)
                                                  .ThenInclude(n => n.Image)
                                                  .Skip(skip)
                                                  .Take(take)
                                                  .ToListAsync();
            return View(products);
        }
    }
}
