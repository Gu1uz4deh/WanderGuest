using WanderQuest.Application.Services.Public;
using WanderQuest.Infrastructure.Models;
using System;
using WanderQuest.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using WanderQuest.Infrastructure.Abstracts;

namespace WanderQuest.Application.Implementations.Public
{
    public class ProductsQueryRepository : IProductsQueryService
    {
        private readonly IProductDAL _repository;
        public ProductsQueryRepository(IProductDAL repository)
        {
            _repository = repository;
        }
        public async Task<List<Product>> GetAll()
        {
            var products = await _repository.GetAllDetailedAsync();

            return products;
        }

        public async Task<Product> GetById(int id)
        {
            var product = await _repository.GetDetailedByIdAsync(id);
            return product;
        }
        public async Task<List<Product>> GetPaged(int skip = 0, int take = 10)
        {
            var products = await _repository.GetPagedDetailedAsync(skip, take);
            return products;
        }

        public async Task<List<Product>> SearchForTitle(string title)
        {

            var findingProducts = await _repository.SearchPagedDetailedAsync(title);

            return findingProducts;
        }
        public async Task<List<Product>> GetForCategoryAsync(int categoryId = 1, int skip = 0, int take = 4)
        {
            var findingProducts = await _repository.GetByCategoryPagedAsync(categoryId, skip, take);
            return findingProducts;
        }

    }
}
