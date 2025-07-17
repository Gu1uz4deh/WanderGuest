using System;
using WanderQuest.ViewModels.Basket;

namespace WanderQuest.BasketHandlers.Services
{
    public interface IBasketSummaryService
    {
        Task<BasketSummaryVM> GetSummaryAsync();
    }
}
