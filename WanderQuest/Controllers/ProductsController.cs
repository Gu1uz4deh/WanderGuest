using System;
using WanderQuest.Infrastructure.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WanderQuest.ViewModel;
using System.Text.Json;
using WanderQuest.Application.Services.Public;

namespace WanderQuest.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsQueryService _productQueryService;

        public ProductsController(IProductsQueryService products)
        {
            _productQueryService = products;
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult SetBasket(int id)
        {
            List<BasketItemVM> basketItems;

            var cookieString = Request.Cookies["basket"];

            if (string.IsNullOrEmpty(cookieString))
            {
                basketItems = new List<BasketItemVM>();
            }
            else
            {
                basketItems = JsonSerializer.Deserialize<List<BasketItemVM>>(cookieString);
            }

            var basketItem = basketItems.FirstOrDefault(n => n.Id == id);

            if (basketItem == null)
            {
                basketItems.Add(new BasketItemVM()
                {
                    Id = id,
                    Count = 1
                });
            }
            else
            {
                basketItem.Count++;
            }

            cookieString = JsonSerializer.Serialize(basketItems);

            Response.Cookies.Append("basket", cookieString);

            return Json(new
            {
                status = 200,
                data = basketItems
            });
        }

        public IActionResult GetBasket()
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
            return Json(new
            {
                status = 200,
                data = basketItems
            });
        }

        #region CookieTest
        //public IActionResult Set()
        //{
        //    //HttpContext.Session.SetString("test", "tural");

        //    Response.Cookies.Append("test", "tural", new CookieOptions()
        //    {
        //        MaxAge = TimeSpan.FromSeconds(5)
        //    });

        //    return Json("OK");
        //}

        //public IActionResult Get()
        //{
        //    //var data = HttpContext.Session.GetString("test");

        //    var data = Request.Cookies["test"];

        //    return Json(data);
        //}
        #endregion
    }
}
