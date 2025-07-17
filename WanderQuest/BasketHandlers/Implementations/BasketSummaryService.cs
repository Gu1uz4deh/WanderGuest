using System;
using WanderQuest.BasketHandlers.Services;
using WanderQuest.ViewModels.Basket;

namespace WanderQuest.BasketHandlers.Implementations
{
    public class BasketSummaryService : IBasketSummaryService
    {
        private readonly IBasketItemService _basketItemService;

        public BasketSummaryService(IBasketItemService basketItemService)
        {
            _basketItemService = basketItemService;
        }
        public async Task<BasketSummaryVM> GetSummaryAsync()
        {
            var basketItems = await _basketItemService.GetBasketItemDetails();

            BasketSummaryVM basketSummary = new BasketSummaryVM();
            basketSummary.TotalItems = 0;
            basketSummary.TotalPrice = 0;

            if (basketItems.Count > 0 )
            {
                foreach (var basketItem in basketItems)
                {
                    basketSummary.TotalItems = basketSummary.TotalItems + basketItem.Count;
                    basketSummary.TotalPrice = basketSummary.TotalPrice + basketItem.Price * basketItem.Count;
                }
            }

            return basketSummary;
        }
    }
}
