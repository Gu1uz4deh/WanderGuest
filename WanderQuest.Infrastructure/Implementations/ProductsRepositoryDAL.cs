using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WanderQuest.Core.EFRepository.EFEntityRepositoryBase;
using WanderQuest.Infrastructure.Abstracts;
using WanderQuest.Infrastructure.DAL;
using WanderQuest.Infrastructure.Models;

namespace WanderQuest.Infrastructure.Implementations
{
    public class ProductsRepositoryDAL : EFEntityRepositoryBase<Product, AppDbContext>, IProductDAL
    {
        public ProductsRepositoryDAL(AppDbContext context) : base(context) { }

        private IQueryable<Product> GetBaseProductQuery()
        {
            return _context.Products
                .Where(p => !p.IsDeleted)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .ThenInclude(pi => pi.Image);
        }
        public async Task<List<Product>> GetAllDetailedAsync()
        {
            return await GetBaseProductQuery().ToListAsync();
        }
        public async Task<Product> GetDetailedByIdAsync(int id)
        {
            return await GetBaseProductQuery().FirstOrDefaultAsync(n => n.Id == id);
        }
        public async Task<List<Product>> GetPagedDetailedAsync(int skip = 0, int take = 4)
        {
            return await GetBaseProductQuery()
                                   .OrderByDescending(n => n.UpdatedDate)
                                   .Skip(skip)
                                   .Take(take)
                                   .ToListAsync();
        }
        public async Task<List<Product>> SearchPagedDetailedAsync(string title, int skip = 0, int take = 4)
        {
            return await GetBaseProductQuery().Where(n => n.Title.ToLower().Contains(title.ToLower()))
                                   .OrderByDescending(n => n.UpdatedDate)
                                   .Skip(skip)
                                   .Take(take)
                                   .ToListAsync();
        }
        public async Task<List<Product>> GetByCategoryPagedAsync(int categoryId = 1, int skip = 0, int take = 4)
        {
            return await GetBaseProductQuery().Where(n => n.CategoryId == categoryId)
                                   .OrderByDescending(n => n.UpdatedDate)
                                   .Skip(skip)
                                   .Take(take)
                                   .ToListAsync();
        }
    }
}
