using WanderQuest.Infrastructure.Models;
using System;

namespace WanderQuest.Application.Services.Public
{
    public interface IProductsQueryService
    {
        Task<List<Product>> GetAll();
        Task<Product> GetById(int id);
        Task<List<Product>> GetPaged(int skip = 0, int take = 4);
    }
}
