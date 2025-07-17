using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;  // NullView üçün
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using WanderQuest.Application.Services.Public;
using WanderQuest.BasketHandlers.Services;
using WanderQuest.Infrastructure.DAL;
using WanderQuest.Infrastructure.Models;
using WanderQuest.Shared.Helpers;
using WanderQuest.ViewModels;

namespace WanderQuest.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsQueryService _productQueryService;
        private readonly IBasketItemService _basketItemService;

        public ProductsController(IProductsQueryService productQueryService,
                                  IBasketItemService basketItemService)
        {
            _productQueryService = productQueryService;
            _basketItemService = basketItemService;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _productQueryService.GetPaged(0, 4);

            return View(products);
        }

        [Route("{controller}/loadmore/{skip}")]
        public async Task<IActionResult> LoadMore(int skip)
        {
            var products = await _productQueryService.GetPaged(skip, 4);
            return PartialView("_ProductPartial", products);
        }

        public async Task<IActionResult> SearchProduct(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                title = "Trek";
            }

            var findingProducts = await _productQueryService.SearchForTitle(title);

            ViewBag.SearchingWord = title;

            return View(findingProducts);
        }


        [Authorize]
        [HttpGet("products/setbasket/{productId}")]
        public async Task<IActionResult> SetBasket(int productId)
        {
            await _basketItemService.AddBasketItem(productId);
            
            return Json(new
            {
                status = 200
            });
        }

        #region LoadCategoryProductsWithPartilView
        //[Route("products/LoadCategoryProductsHtml/{categoryId}/{skip}")]
        //public async Task<IActionResult> LoadCategoryProductsHtml(int categoryId, int skip)
        //{
        //    if (categoryId == 0)
        //    {
        //        var allProducts = await _productQueryService.GetPaged(skip, 4);
        //        return PartialView("_ProductPartial", allProducts);
        //    }
        //    var products = await _productQueryService.GetForCategoryAsync(categoryId, skip);
        //    return PartialView("_ProductPartial", products);
        //}
        #endregion
        [HttpGet("products/LoadCategoryProductsHtml")]
        public async Task<IActionResult> LoadCategoryProductsHtml(int categoryId, int skip, int take)
        {
            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                { "CategoryId", categoryId },
                { "Skip", skip },
                { "Take", take }
            };

            var actionContext = new ActionContext(HttpContext, RouteData, ControllerContext.ActionDescriptor);

            var nullView = new NullView();

            var viewContext = new ViewContext(
                actionContext,
                nullView,
                viewData,
                TempData,
                TextWriter.Null,
                new HtmlHelperOptions()
            );

            //string html = await ControllerExtensions.RenderAsync(HttpContext, viewContext, "ProductsByCategory", null);
            string html = await ControllerExtensions.RenderAsync(
                HttpContext,
                viewContext,
                "ProductsByCategory",
                new { categoryId = categoryId, skip = skip, take = take }
            );
            return Json(new { html });
        }

        public class NullView : IView
        {
            public string Path => string.Empty;

            public Task RenderAsync(ViewContext context)
            {
                return Task.CompletedTask;
            }
        }

    }
}
