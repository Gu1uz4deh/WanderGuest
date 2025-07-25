using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WanderQuest.Core.EFRepository;
using WanderQuest.Infrastructure.Models;

namespace WanderQuest.Infrastructure.Abstracts
{
    public interface IProductDAL : IRepositoryBase<Product>
    {
        Task<List<Product>> GetAllDetailedAsync();
        Task<Product> GetDetailedByIdAsync(int id);
        Task<List<Product>> GetPagedDetailedAsync(int skip, int take);

        Task<List<Product>> SearchPagedDetailedAsync(string title, int skip = 0, int take = 4);
        Task<List<Product>> GetByCategoryPagedAsync(int categoryId = 0, int skip = 0, int take = 4);
    }
}
