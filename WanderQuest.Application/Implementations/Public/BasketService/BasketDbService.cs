using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WanderQuest.Application.Services.Public.BasketService;
using WanderQuest.Infrastructure.DAL;
using WanderQuest.Infrastructure.Models;

namespace WanderQuest.Application.Implementations.Public.BasketService
{
    public class BasketDbService : IBasketDbService
    {
        private readonly AppDbContext _context;
        public BasketDbService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BasketItem>> GetBasketItems(string userId)
        {
            var basket = await CreateBasketIfNotExistAsync(userId);

            var basketItems = await _context.BasketItems.Where(n => n.BasketId == basket.Id).ToListAsync();

            if (basketItems is null)
            {
                basketItems = new List<BasketItem>();
            }

            return basketItems;
        }

        public async Task AddBasketItem(string userId, int productId)
        {
            var basket = await CreateBasketIfNotExistAsync(userId);

            var basketItems = await GetBasketItems(userId);

            var basketItem = basketItems.FirstOrDefault(n => n.ProductId == productId);
            if (basketItem == null)
            {
                var newBasketItem = new BasketItem()
                {
                    ProductId = productId,
                    Quantity = 1,
                    BasketId = basket.Id,
                    AddingDate = DateTime.Now,
                };
                await _context.BasketItems.AddAsync(newBasketItem);
            }
            else
            {
                basketItem.Quantity++;
                basketItem.AddingDate = DateTime.Now;
            }
            await _context.SaveChangesAsync();


        }
        public async Task<Basket> CreateBasketIfNotExistAsync(string userId)
        {
            var basket = await FindBasketAsync(userId);
            if (basket is null)
            {
                basket = new Basket();
                basket.AppUserId = userId;
                await _context.Baskets.AddAsync(basket);
                await _context.SaveChangesAsync();
            }
            return basket;
        }



        public async Task SetBasketItemQuantity(string userId, int productId, int quantity)
        {
            var basketItems = await GetBasketItems(userId);

            var basketItem = basketItems.FirstOrDefault(n => n.ProductId == productId);

            if (basketItem != null)
            {
                if (quantity == null || quantity <= 0)
                {
                    _context.BasketItems.Remove(basketItem);
                }
                if (quantity > 100)
                {
                    quantity = 100;
                }
                basketItem.Quantity = quantity;
                _context.BasketItems.Update(basketItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateBasketItemQuantity(string userId, int productId, int quantity)
        {
            var basketItems = await GetBasketItems(userId);

            if (quantity == null)
            {
                quantity = 0;
            }
            if (quantity > 100)
            {
                quantity = 100;
            }
            if (quantity < -100)
            {
                quantity = -100;
            }

            var basketItem = basketItems.FirstOrDefault(n => n.ProductId == productId);

            if (basketItem != null)
            {
                basketItem.Quantity = basketItem.Quantity + quantity;
                if (basketItem.Quantity > 100)
                {
                    basketItem.Quantity = 100;
                }
                if (basketItem.Quantity <= 0)
                {
                    _context.BasketItems.Remove(basketItem);
                }
                else
                {
                    _context.BasketItems.Update(basketItem);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBasketItem(string userId, int productId)
        {
            var basketItems = await GetBasketItems(userId);

            var basketItem = basketItems.FirstOrDefault(n => n.ProductId == productId);

            if (basketItem != null)
            {
                _context.BasketItems.Remove(basketItem);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<bool> BasketIsExist(string userId)
        {
            var basket = await _context.Baskets.Where(n => n.AppUserId == userId).FirstOrDefaultAsync();

            if (basket is null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<Basket> FindBasketAsync(string userId)
        {
            return await _context.Baskets.Where(n => n.AppUserId == userId).FirstOrDefaultAsync();
        }


    }
}
