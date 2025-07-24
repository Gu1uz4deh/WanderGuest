using Microsoft.EntityFrameworkCore;
using System;
using WanderQuest.Application.Services.Public;
using WanderQuest.Infrastructure.Abstracts;
using WanderQuest.Infrastructure.DAL;
using WanderQuest.Infrastructure.Models;

namespace WanderQuest.Application.Implementations.Public
{
    public class CategoriesQueryRepository : ICategoriesQueryService
    {
        private readonly ICategoryDAL _repository;
        public CategoriesQueryRepository(ICategoryDAL repository)
        {
            _repository = repository;
        }
        public async Task<List<Category>> GetAll()
        {
            var categories = await _repository.GetAllAsync(n => !n.IsDeleted);
            return categories;
        }

        public async Task<Category> GetById(int id)
        {
            var category = await _repository.GetAsync(n => !n.IsDeleted && n.Id == id);
            return category;
        }
    }
}
