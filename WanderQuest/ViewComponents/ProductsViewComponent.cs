using System;
using WanderQuest.Infrastructure.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WanderQuest.Application.Services.Public;

namespace WanderQuest.ViewComponents
{
    public class ProductsViewComponent : ViewComponent
    {
        private readonly IProductsQueryService _productQueryService;
        public ProductsViewComponent(IProductsQueryService productQueryService)
        {
            _productQueryService = productQueryService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _productQueryService.GetPaged();
            return View(products);
        }
    }
}
