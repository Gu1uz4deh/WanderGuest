using System;
using System;
using WanderQuest.Infrastructure.Models;

namespace WanderQuest.Application.Services.Public.BasketService
{
    public interface IBasketDbService
    {
        Task<List<BasketItem>> GetBasketItems(string userId);
        Task AddBasketItem(string userId, int productId);
        Task<Basket> CreateBasketIfNotExistAsync(string userId);
        Task SetBasketItemQuantity(string userId, int productId, int quantity);
        Task UpdateBasketItemQuantity(string userId, int productId, int quantity);
        Task DeleteBasketItem(string userId, int productId);
        Task<bool> BasketIsExist(string userId);
        Task<Basket> FindBasketAsync(string userId);
    }
}
