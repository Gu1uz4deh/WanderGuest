using WanderQuest.Shared.Exceptions;
using WanderQuest.Infrastructure.DAL;
using WanderQuest.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WanderQuest.Application.Services.Admin;

namespace WanderQuest.Application.Implementations.Admin
{
    public class CategoryAdminRepository : ICategoryAdminService
    {
        private readonly AppDbContext _context;

        public CategoryAdminRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Category> Get(int id)
        {
            var data = await _context.Categories.Where(n => !n.IsDeleted && n.Id == id)
                                                .FirstOrDefaultAsync();
            return data;
        }
        public async Task<List<Category>> GetAll()
        {
            var data = await _context.Categories.Where(n => !n.IsDeleted).ToListAsync();
            return data;
        }
        public async Task Create(Category category)
        {
            category.CreatedDate = DateTime.Now;

            try
            {
                await _context.Categories.AddAsync(category);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var data = await Get(id);
            data.IsDeleted = true;
            _context.Categories.Update(data);
            await _context.SaveChangesAsync();
        }

        public Task<Category> Details(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Update(int id, string categoryName)
        {
            var data = await Get(id);
            
            if(data is null || string.IsNullOrEmpty(categoryName.Trim()))
            {
                throw new EntityIsNotExistException();
            }
            data.Name = categoryName;
            data.UpdatedDate = DateTime.Now;
            _context.Categories.Update(data);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsExist(string name)
        {
            bool isExist = await _context.Categories.AnyAsync(n => n.Name.Trim().ToLower() == name.Trim().ToLower());
            return isExist;
        }
    }
}
