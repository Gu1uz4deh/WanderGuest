using System;
using System.Text.Json;
using System.Threading.Tasks;
using Data.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WanderQuest.ViewModel;

namespace WanderQuest.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            decimal totalPrice = 0;
            List<BasketItemVM> basketItems = GetBasketCookie();

            List<BasketVM> baskets = new List<BasketVM>();

            for (int i = 0; i < basketItems.Count; i++)
            {
                var data = await _context.Products.Where(n => !n.IsDeleted && n.Id == basketItems[i].Id)
                                                   .Include(n => n.Category)
                                                   .Include(n => n.ProductImages)
                                                   .ThenInclude(n => n.Image)
                                                   .FirstOrDefaultAsync();
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
    }
}
