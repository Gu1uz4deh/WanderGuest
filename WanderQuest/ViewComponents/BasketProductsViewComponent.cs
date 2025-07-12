using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using WanderQuest.Application.Services.Public;
using WanderQuest.ViewModel;

namespace WanderQuest.ViewComponents
{
    public class BasketProductsViewComponent : ViewComponent
    {
        private readonly IProductsQueryService _productsQueryService;
        public BasketProductsViewComponent(IProductsQueryService productsQueryService)
        {
            _productsQueryService = productsQueryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            decimal totalPrice = 0;
            List<BasketItemVM> basketJson;
            string cookieString = Request.Cookies["basket"];
            if (string.IsNullOrEmpty(cookieString))
            {
                basketJson = new List<BasketItemVM>();
            }
            else
            {
                basketJson = JsonSerializer.Deserialize<List<BasketItemVM>>(cookieString).OrderByDescending(x => x.AddingDate).ToList();
            }

            List<BasketVM> productsInfo = new List<BasketVM>();

            for (int i = 0; i < basketJson.Count; i++)
            {
                var dbProduct = await _productsQueryService.GetById(basketJson[i].Id);

                if (dbProduct != null)
                {
                    totalPrice = totalPrice + dbProduct.Price * basketJson[i].Count;
                    productsInfo.Add(new BasketVM
                    {
                        Id = dbProduct.Id,
                        ImageUrl = dbProduct.ProductImages[0].Image.Name,
                        Title = dbProduct.Title,
                        Price = dbProduct.Price,
                        Category = dbProduct.Category.Name,
                        Count = basketJson[i].Count
                    });
                }
            }

            ViewBag.TotalPrice = totalPrice;
            return View(productsInfo);
        }
    }
}
