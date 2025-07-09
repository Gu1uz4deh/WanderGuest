using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;  // NullView üçün
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using WanderQuest.Application.Services.Public;
using WanderQuest.Infrastructure.DAL;
using WanderQuest.Shared.Helpers;
using WanderQuest.ViewModel;

namespace WanderQuest.Controllers
{
    public class BasketController : Controller
    {
        private readonly IProductsQueryService _productQueryService;

        public BasketController(IProductsQueryService productQueryService)
        {
            _productQueryService = productQueryService;
        }
        public async Task<IActionResult> Index()
        {
            decimal totalPrice = 0;
            List<BasketItemVM> basketItems = GetBasketCookie();

            List<BasketVM> baskets = new List<BasketVM>();

            for (int i = 0; i < basketItems.Count; i++)
            {
                var data = await _productQueryService.GetById(basketItems[i].Id);
                if (data != null)
                {
                    totalPrice = totalPrice + data.Price * basketItems[i].Count;
                    baskets.Add(new BasketVM
                    {
                        ImageUrl = data.ProductImages[0].Image.Name,
                        Title = data.Title,
                        Price = data.Price,
                        Category = data.Category.Name,
                        Count = basketItems[i].Count
                    });
                }
            }

            ViewBag.TotalPrice = totalPrice;
            return View(baskets);
        }

        public List<BasketItemVM> GetBasketCookie()
        {
            List<BasketItemVM> basketItems;
            string cookieString = Request.Cookies["basket"];
            if (string.IsNullOrEmpty(cookieString))
            {
                basketItems = new List<BasketItemVM>();
            }
            else
            {
                basketItems = JsonSerializer.Deserialize<List<BasketItemVM>>(cookieString);

            }
            return basketItems;
        }

        public async Task<IActionResult> GetHoverDetailsHtml()
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
            string html = await ControllerExtensions.RenderAsync(HttpContext, viewContext, "BasketHoverDetails", null);

            // 5. Nəticəni view-ə göndər
            //ViewBag.BasketHtml = html;

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
