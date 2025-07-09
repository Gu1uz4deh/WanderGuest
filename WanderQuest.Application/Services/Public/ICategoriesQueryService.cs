using System;
using WanderQuest.Infrastructure.Models;

namespace WanderQuest.Application.Services.Public
{
    public interface ICategoriesQueryService 
    {
        Task<List<Category>> GetAll();
        Task<Category> GetById(int id);
    }
}
