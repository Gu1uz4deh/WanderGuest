using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
using WanderQuest.ViewModels.Basket;

namespace WanderQuest.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly IProductsQueryService _productQueryService;
        private readonly IBasketItemService _basketItemService;
        private readonly IBasketSummaryService _basketSummaryService;

        public BasketController(IProductsQueryService productQueryService,
                                IBasketItemService basketCookieService,
                                IBasketSummaryService basketSummaryService)
        {
            _productQueryService = productQueryService;
            _basketItemService = basketCookieService;
            _basketSummaryService = basketSummaryService;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var products = await _basketItemService.GetBasketItemDetails();

            BasketSummaryVM basketSummary = await _basketSummaryService.GetSummaryAsync();

            ViewBag.TotalPrice = basketSummary.TotalPrice;
            return View(products);
        }

        [HttpGet("basket/DecreaseProductQuantity/{productId}")]
        public async Task<IActionResult> DecreaseProductQuantity(int productId)
        {
            await _basketItemService.UpdateBasketItemQuantity(productId, -1);
            return Json(new { status = 200 });
        }

        [HttpGet("basket/IncreaseProductQuantity/{productId}")]
        public async Task<IActionResult> IncreaseProductQuantity(int productId)
        {
            await _basketItemService.UpdateBasketItemQuantity(productId, 1);
            return Json(new { status = 200 });
        }

        [HttpGet("basket/UpdateProductQuantity/{productId}/{quantity}")]
        public async Task<IActionResult> UpdateProductQuantity(int productId, int quantity)
        {
            await _basketItemService.SetBasketItemQuantity(productId, quantity);
            return Json(new { status = 200 });
        }

        [HttpGet("basket/DeleteProduct/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            await _basketItemService.DeleteBasketItem(productId);
            return Json(new { status = 200 });
        }

        [HttpPost("basket/GetBasketSummary")]
        public async Task<IActionResult> GetBasketSummary()
        {
            BasketSummaryVM basketSummaryVM = await _basketSummaryService.GetSummaryAsync();

            return Json(basketSummaryVM);
        }





        public async Task<IActionResult> GetBasketProductsHtml()
        {
            // 1. ActionContext hazırla
            var actionContext = new ActionContext(HttpContext, RouteData, ControllerContext.ActionDescriptor);

            // 2. NullView sinifindən nümunə yarat (aşağıda necə yaradacağını göstərəcəyəm)
            var nullView = new NullView();

            // 3. ViewContext yarat
            var viewContext = new ViewContext(
                actionContext,
                nullView,
                ViewData,
                TempData,
                TextWriter.Null,
                new HtmlHelperOptions()
            );

            // 4. Sənin RenderAsync metodunu çağır
            string html = await ControllerExtensions.RenderAsync(HttpContext, viewContext, "BasketProducts", null);

            // 5. Nəticəni view-ə göndər
            //ViewBag.BasketHtml = html;

            return Json(new { html });
        }

        public async Task<IActionResult> GetHoverDetailsHtml()
        {
            var actionContext = new ActionContext(HttpContext, RouteData, ControllerContext.ActionDescriptor);

            var nullView = new NullView();

            var viewContext = new ViewContext(
                actionContext,
                nullView,
                ViewData,
                TempData,
                TextWriter.Null,
                new HtmlHelperOptions()
            );

            string html = await ControllerExtensions.RenderAsync(HttpContext, viewContext, "BasketHoverDetails", null);

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
