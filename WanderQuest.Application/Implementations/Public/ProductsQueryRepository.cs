using WanderQuest.Application.Services.Public;
using WanderQuest.Infrastructure.Models;
using System;
using WanderQuest.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace WanderQuest.Application.Implementations.Public
{
    public class ProductsQueryRepository : IProductsQueryService
    {
        private readonly AppDbContext _context;
        public ProductsQueryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetAll()
        {
            var products = await _context.Products.Where(n => !n.IsDeleted)
                                              .Include(n => n.Category)
                                              .Include(n => n.ProductImages)
                                              .ThenInclude(n => n.Image)
                                              .ToListAsync();

            return products;
        }

        public async Task<Product> GetById(int id)
        {
            var product = await _context.Products.Where(n => !n.IsDeleted && n.Id == id)
                                        .Include(n => n.Category)
                                        .Include(n => n.ProductImages)
                                        .ThenInclude(n => n.Image)
                                        .FirstOrDefaultAsync();
            return product;
        }
        public async Task<List<Product>> GetPaged(int skip = 0, int take = 10)
        {
            var products = await _context.Products.Where(n => !n.IsDeleted)
                                              .Include(n => n.Category)
                                              .Include(n => n.ProductImages)
                                              .ThenInclude(n => n.Image)
                                              .Skip(skip)
                                              .Take(take)
                                              .ToListAsync();
            return products;
        }

        public async Task<List<Product>> SearchForTitle(string title)
        {

            var findingProducts = await _context.Products.Where(n => !n.IsDeleted && n.Title.ToLower().Contains(title.ToLower()))
                                                        .Include(n => n.Category)
                                                        .Include(n => n.ProductImages)
                                                        .ThenInclude(n => n.Image)
                                                        .ToListAsync();

            return findingProducts;
        }

    }
}
