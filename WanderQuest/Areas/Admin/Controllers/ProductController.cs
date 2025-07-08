using WanderQuest.Infrastructure.DAL;
using WanderQuest.Infrastructure.Models;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(AppDbContext context, 
                                  IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
            ViewData["Categories"] = await GetCategories();
            bool error = false;
            ModelState.Clear();
            if (string.IsNullOrEmpty(product.Title)) { ModelState.AddModelError("Title", "Title cannot be empty"); error = true; }
            if (string.IsNullOrEmpty(product.Description)) { ModelState.AddModelError("Description", "Description cannot be empty"); error = true; }
            if (product.Price <= 0) { ModelState.AddModelError("Price", "Price cannot under 0!"); error = true; }
            if (product.ImageFile is null) { ModelState.AddModelError("ImageFile", "Image field cannot be null"); error = true; }
            if (error) { return View(product); }
            if (!product.ImageFile.ContentType.Contains("image/")) { ModelState.AddModelError("ImageFile", "File must be image only"); return View(product); }

            decimal size = (decimal) product.ImageFile.Length / 1024 / 1024;

            if (size > 3)
            {
                ModelState.AddModelError("ImageFile", "File size cannot  be more than 3 MB"); 
                return View(product);
            }

            string imageName = Guid.NewGuid().ToString() + product.ImageFile.FileName;
            if (imageName.Length >= 254)
            {
                imageName = imageName.Substring(imageName.Length - 254, 254);
            }

            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "uploads", "products", imageName);

            using (FileStream filestream = new FileStream(path, FileMode.Create))
            {
                await product.ImageFile.CopyToAsync(filestream);
            }


            Image image = new Image();
            image.Name = imageName;
            

            product.CreatedDate = DateTime.Now;
            await _context.Images.AddAsync(image);
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
            ModelState.Clear();

            var dbProduct = await _context.Products.Where(n => !n.IsDeleted && n.Id == id)
                .Include(n => n.Category)
                .Include(n => n.ProductImages)
                .ThenInclude(n => n.Image)
                .FirstOrDefaultAsync();
            if (dbProduct == null)
            {
                return NotFound();
            }

            if (product.ImageFile != null)
            {
                decimal size = (decimal) product.ImageFile.Length / 1024 / 1024;
                if (!product.ImageFile.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("ImageFile", "File must be image only");
                    return View(product);
                }
                if (size > 3)
                {
                    ModelState.AddModelError("ImageFile", "Image size cannot be more than 3 MB");
                    return View(product);
                }

                string fileName = Guid.NewGuid().ToString() + product.ImageFile.FileName;

                if (fileName.Length > 254)
                {
                    fileName = fileName.Substring(fileName.Length - 254, 254);
                }

                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "uploads", "products", fileName);

                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(fileStream);
                }

                Image image = new Image();
                image.Name = fileName;

                await _context.Images.AddAsync(image);

                var oldProductImage = await _context.ProductImages.Where(n => n.ProductId == dbProduct.Id)
                                                                .FirstOrDefaultAsync();

                if (oldProductImage == null)
                {
                    return Json("ProductImage is null");
                }

                var oldImage = await _context.Images.Where(n => n.Id == oldProductImage.ImageId).FirstOrDefaultAsync();

                if (oldImage == null)
                {
                    return Json("Image is null");
                }

                _context.ProductImages.Remove(oldProductImage);
                await _context.SaveChangesAsync();

                _context.Images.Remove(oldImage);

                ProductImages newProductImage = new ProductImages();
                newProductImage.ImageId = image.Id;
                newProductImage.ProductId = dbProduct.Id;

                await _context.ProductImages.AddAsync(newProductImage);
                await _context.SaveChangesAsync();
            }


            dbProduct.CategoryId = product.CategoryId;
            dbProduct.Title = product.Title;
            dbProduct.Description = product.Description;
            dbProduct.Price = product.Price;

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
            if(dbProduct.Title.Trim().ToLower() != product.Title.Trim().ToLower())
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
