using WanderQuest.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WanderQuest.Application.Services.Admin
{
    public interface ICategoryAdminService
    {
        Task<Category> Get(int id);
        Task<List<Category>> GetAll();
        Task<Category> Details(int id);
        Task Update(int id, string categoryName);
        Task Create(Category category);
        Task Delete(int id);
        Task<bool> IsExist(string name);
    }
}
