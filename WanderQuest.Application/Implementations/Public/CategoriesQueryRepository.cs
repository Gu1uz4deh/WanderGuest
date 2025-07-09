using Microsoft.EntityFrameworkCore;
using System;
using WanderQuest.Application.Services.Public;
using WanderQuest.Infrastructure.DAL;
using WanderQuest.Infrastructure.Models;

namespace WanderQuest.Application.Implementations.Public
{
    public class CategoriesQueryRepository : ICategoriesQueryService
    {
        private readonly AppDbContext _context;
        public CategoriesQueryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Category>> GetAll()
        {
            var categories = await _context.Categories.Where(n => !n.IsDeleted).ToListAsync();
            return categories;
        }

        public async Task<Category> GetById(int id)
        {
            var category = await _context.Categories.Where(n => !n.IsDeleted && n.Id == id).FirstOrDefaultAsync();
            return category;
        }
    }
}
