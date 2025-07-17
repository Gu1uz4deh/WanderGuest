using System;
using WanderQuest.ViewModels;

namespace WanderQuest.BasketHandlers.Services
{
    public interface IBasketItemService
    {
        Task<List<BasketItemVM>> GetBasketItems();
        Task<List<BasketVM>> GetBasketItemDetails();
        Task AddBasketItem(int productId);
        Task SetBasketItemQuantity(int productId, int quantity);
        Task UpdateBasketItemQuantity(int productId, int quantity);
        Task DeleteBasketItem(int productId);
        Task CreateBasketIfNotExistAsync(string userId);
    }
}
