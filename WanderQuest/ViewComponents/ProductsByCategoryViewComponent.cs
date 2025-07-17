using Microsoft.AspNetCore.Mvc;
using System;
using WanderQuest.Application.Services.Public;
using WanderQuest.Infrastructure.Models;

namespace WanderQuest.ViewComponents
{
    public class ProductsByCategoryViewComponent : ViewComponent
    {
        private readonly IProductsQueryService _productsQueryService;

        public ProductsByCategoryViewComponent(IProductsQueryService productsQueryService)
        {
            _productsQueryService = productsQueryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int categoryId , int skip, int take )
        {
            if (categoryId == 0)
            {
                var allProducts = await _productsQueryService.GetPaged(skip, 6);
                return View(allProducts);
            }
            var products = await _productsQueryService.GetForCategoryAsync(categoryId, skip, take);
            return View(products);
        }
    }
}
