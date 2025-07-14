using Microsoft.AspNetCore.Authorization;
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
using WanderQuest.Infrastructure.Models;
using WanderQuest.Shared.Helpers;
using WanderQuest.ViewModels;

namespace WanderQuest.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly IProductsQueryService _productQueryService;

        public BasketController(IProductsQueryService productQueryService)
        {
            _productQueryService = productQueryService;
        }
        [AllowAnonymous]
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
                        Id = data.Id,
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
        public IActionResult DecreaseProductQuantity(int id)
        {
            UpdateProductQuantityMethod(id, -1);
            
            return Json(new
            {
                status = 200
            });
        }
        public IActionResult IncreaseProductQuantity(int id)
        {
            UpdateProductQuantityMethod(id, 1);

            return Json(new
            {
                status = 200
            });
        }
        public IActionResult DeleteProduct(int id)
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

            var basketItem = basketItems.Where(n => n.Id == id).FirstOrDefault();

            if (basketItem != null)
            {
                basketItems.Remove(basketItem);

                cookieString = JsonSerializer.Serialize(basketItems);
                Response.Cookies.Append("basket", cookieString);
            }
            else
            {
                return Content("Item Not Found");
            }

            return Json(new
            {
                status = 200,
            });

        }
        public async Task<IActionResult> UpdateProductQuantity(int id, int quantity)
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

            var basketItem = basketItems.Where(n => n.Id == id).FirstOrDefault();

            if (basketItem != null)
            {
                try
                {
                    basketItem.Count = quantity;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                if (basketItem.Count <= 0)
                {
                    basketItems.Remove(basketItem);
                }
                if (basketItem.Count > 200000)
                {
                    basketItem.Count = 200000;
                }


                cookieString = JsonSerializer.Serialize(basketItems);

                Response.Cookies.Append("basket", cookieString);

            }
            else
            {
                return Content("Item Not Found");
            }

            return Json(new
            {
                status = 200,
            });
        }
        public async Task<IActionResult> UpdateProductQuantityMethod(int id, int quantity)
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

            var basketItem = basketItems.Where(n => n.Id == id).FirstOrDefault();

            if (basketItem != null)
            {
                try
                {
                    basketItem.Count += quantity;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                if (basketItem.Count <= 0)
                {
                    basketItems.Remove(basketItem);
                }
                if (basketItem.Count > 200000)
                {
                    basketItem.Count = 200000;
                }


                cookieString = JsonSerializer.Serialize(basketItems);

                Response.Cookies.Append("basket", cookieString);

            }
            else
            {
                return Content("Item Not Found");
            }

            return Json(new
            {
                status = 200,
            });
        }

        public async Task<List<BasketItemVM>> GetAllBasketProducts()
        {
            List<BasketItemVM> basketItems;

            string cookieString = Request.Cookies["basket"];

            if (string.IsNullOrEmpty(cookieString))
            {
                return null;
            }
            else
            {
                basketItems = JsonSerializer.Deserialize<List<BasketItemVM>>(cookieString);
            }

            return basketItems;
        }
        public async Task<BasketItemVM> GetBasketProduct(int id)
        {
            List<BasketItemVM> basketItems;

            string cookieString = Request.Cookies["basket"];

            if (string.IsNullOrEmpty(cookieString))
            {
                return null;
            }
            else
            {
                basketItems = JsonSerializer.Deserialize<List<BasketItemVM>>(cookieString);
            }

            var basketItem = basketItems.Where(n => n.Id == id).FirstOrDefault();
            return basketItem;
        }


        public async Task<IActionResult> TotalBasketItemsInformation()
        {
            TotalBasketItemsVM totalBasketProductInformation = new TotalBasketItemsVM();
            totalBasketProductInformation.TotalCount = 0;
            totalBasketProductInformation.TotalPrice = 0;

            List<BasketItemVM> basketItems;

            string cookieString = Request.Cookies["basket"];

            if (!string.IsNullOrEmpty(cookieString))
            {
                basketItems = JsonSerializer.Deserialize<List<BasketItemVM>>(cookieString);

                for (int i = 0; i < basketItems.Count; i++)
                {
                    var basketItem = await _productQueryService.GetById(basketItems[i].Id); 
                    totalBasketProductInformation.TotalCount = totalBasketProductInformation.TotalCount + basketItems[i].Count;
                    totalBasketProductInformation.TotalPrice = totalBasketProductInformation.TotalPrice + basketItem.Price * basketItems[i].Count;
                }
            }

            return Json(new
            {
                data = totalBasketProductInformation
            });
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
