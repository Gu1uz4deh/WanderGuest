using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using WanderQuest.Application.Services.Public;
using WanderQuest.BasketHandlers.Services;
using WanderQuest.ViewModels;

namespace WanderQuest.ViewComponents
{
    public class BasketProductsViewComponent : ViewComponent
    {
        private readonly IProductsQueryService _productsQueryService;
        private readonly IBasketItemService _basketCookieService;
        public BasketProductsViewComponent(IProductsQueryService productsQueryService, IBasketItemService basketCookieService)
        {
            _productsQueryService = productsQueryService;
            _basketCookieService = basketCookieService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            decimal totalPrice = 0;
            var products = await _basketCookieService.GetBasketItemDetails();

            for (int i = 0; i < products.Count; i++)
            {
                totalPrice = totalPrice + products[i].Count * products[i].Price;
            }

            ViewBag.TotalPrice = totalPrice;
            return View(products);
        }
    }
}
