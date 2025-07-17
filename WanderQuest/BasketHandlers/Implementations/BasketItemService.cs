using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Text.Json;
using WanderQuest.Application.Services.Public;
using WanderQuest.Application.Services.Public.BasketService;
using WanderQuest.BasketHandlers.Services;
using WanderQuest.Infrastructure.Models;
using WanderQuest.ViewModels;

namespace WanderQuest.BasketHandlers.Implementations
{
    public class BasketItemService : IBasketItemService
    {
        private readonly IBasketDbService _basketDbService;
        private readonly IProductsQueryService _productsQueryService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BasketItemService(IBasketDbService basketDbService,
                                   IProductsQueryService productsQueryService,
                                   IHttpContextAccessor httpContextAccessor)
        {
            _basketDbService = basketDbService;
            _productsQueryService = productsQueryService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<BasketItemVM>> GetBasketItems()
        {
            string userId = await GetUserId();
            var userDbItems = await _basketDbService.GetBasketItems(userId);
            userDbItems = userDbItems.OrderByDescending(n => n.AddingDate).ToList(); //sortedByAddingDate

            List<BasketItemVM> basketItems = new List<BasketItemVM>();

            if (userDbItems.Count > 0)
            {
                
                for (int i = 0; i < userDbItems.Count; i++)
                {
                    BasketItemVM basketItem = new BasketItemVM();
                    basketItem.Id = userDbItems[i].ProductId;
                    basketItem.Count = userDbItems[i].Quantity;
                    basketItem.AddingDate = userDbItems[i].AddingDate;
                    basketItems.Add(basketItem);
                }
            }
            return basketItems;
        }
        public async Task<List<BasketVM>> GetBasketItemDetails()
        {
            var basketItems = await GetBasketItems();

            List<BasketVM> productsInfo = new List<BasketVM>();

            for (int i = 0; i < basketItems.Count; i++)
            {
                var product = await _productsQueryService.GetById(basketItems[i].Id);

                if (product != null)
                {
                    productsInfo.Add(new BasketVM
                    {
                        Id = product.Id,
                        ImageUrl = product.ProductImages[0].Image.Name,
                        Title = product.Title,
                        Price = product.Price,
                        Category = product.Category.Name,
                        Count = basketItems[i].Count
                    });
                }
            }
            return productsInfo;
        }

        public async Task AddBasketItem(int productId)
        {
            string userId = await GetUserId();
            await _basketDbService.AddBasketItem(userId, productId);
        }
        public async Task SetBasketItemQuantity(int productId, int quantity)
        {
            string userId = await GetUserId();
            await _basketDbService.SetBasketItemQuantity(userId, productId, quantity);
        }
        public async Task UpdateBasketItemQuantity(int productId, int quantity)
        {
            string userId = await GetUserId();
            await _basketDbService.UpdateBasketItemQuantity(userId, productId, quantity);
        }
        public async Task DeleteBasketItem(int productId)
        {
            string userId = await GetUserId();
            await _basketDbService.DeleteBasketItem(userId, productId);
        }


        public async Task CreateBasketIfNotExistAsync(string userId)
        {
            await _basketDbService.CreateBasketIfNotExistAsync(userId);
        }
        public async Task<string> GetUserId()
        {
            string userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
        }


    }
}
