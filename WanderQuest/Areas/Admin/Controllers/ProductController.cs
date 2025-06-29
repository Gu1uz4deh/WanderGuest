using Data.DAL;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WanderQuest.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Where(n => !n.IsDeleted)
                .Include(n => n.Category)
                .Include(n => n.ProductImages)
                .ThenInclude(n => n.Image)
                .ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.Where(n => !n.IsDeleted && n.Id == id)
                .Include(n => n.Category)
                .Include(n => n.ProductImages)
                .ThenInclude(n => n.Image)
                .FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["Categories"] = await GetCategories();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            
            product.CreatedDate = DateTime.Now;
            var image = await _context.Images.Where(n => n.Name == product.ImageUrl).FirstOrDefaultAsync();

            if (image is null)
            {
                return Content("Image Url Not Found.");
            }

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            ProductImages productImage = new ProductImages();
            productImage.ProductId = product.Id;
            productImage.ImageId = image.Id;

            await _context.ProductImages.AddAsync(productImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Index), controllerName: nameof(Product));
        }

        public async Task<IActionResult> Update(int id)
        {
            ViewData["Categories"] = await GetCategories();

            var product = await _context.Products.Where(n => !n.IsDeleted && n.Id == id)
                .Include(n => n.Category)
                .Include(n => n.ProductImages)
                .ThenInclude(n => n.Image)
                .FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Product product)
        {
            ViewData["Categories"] = await GetCategories();

            var dbProduct = await _context.Products.Where(n => !n.IsDeleted && n.Id == id)
                .Include(n => n.Category)
                .Include(n => n.ProductImages)
                .ThenInclude(n => n.Image)
                .FirstOrDefaultAsync();
            if (dbProduct == null)
            {
                return NotFound();
            }

            var productImage = await _context.ProductImages.Where(n => n.ProductId == dbProduct.Id)
                .FirstOrDefaultAsync();

            var image = await _context.Images.Where(n => n.Name == product.ImageUrl)
                .FirstOrDefaultAsync();

            if (productImage is null || image is null)
            {
                return NotFound();
            }

            productImage.ImageId = image.Id;

            dbProduct.CategoryId = product.CategoryId;
            dbProduct.Title = product.Title;
            dbProduct.Description = product.Description;
            dbProduct.Price = product.Price;

            _context.ProductImages.Update(productImage);
            _context.Products.Update(dbProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction(actionName: nameof(Details), controllerName: nameof(Product), routeValues: new { id });
        }
        
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.Where(n => !n.IsDeleted && n.Id == id)
                .FirstOrDefaultAsync();
            if(product is null) { return NotFound(); }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Product product)
        {
            var dbProduct = await _context.Products.Where(n => !n.IsDeleted && n.Id == product.Id).FirstOrDefaultAsync();
            if(dbProduct is null)
            {
                return NotFound();
            }
            if(dbProduct.Title != product.Title)
            {
                return Content("Cannot Delete");
            }

            dbProduct.IsDeleted = true;

            _context.Products.Update(dbProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction(controllerName:nameof(Product), actionName:nameof(Index));
        }
        private async Task<List<Category>> GetCategories()
        {
            var categories = await _context.Categories.Where(n => !n.IsDeleted).ToListAsync();
            return categories;
        }
    }
}
