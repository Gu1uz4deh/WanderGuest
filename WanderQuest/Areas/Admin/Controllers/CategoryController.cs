using System;
using WanderQuest.Infrastructure.DAL;
using WanderQuest.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WanderQuest.Application.Services.Admin;

namespace WanderQuest.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryAdminService _categoryAdminService;

        public CategoryController(ICategoryAdminService categoryService)
        {
            _categoryAdminService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryAdminService.GetAll(); 
            return View(categories);
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryAdminService.Get(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryAdminService.Get(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, string categoryName)
        {
            await _categoryAdminService.Update(id, categoryName);

            return RedirectToAction(actionName:nameof(Details), controllerName:nameof(Category) , routeValues: new { id });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (await _categoryAdminService.IsExist(category.Name))
            {
                return Content("Name is not available");
            }

            await _categoryAdminService.Create(category);
            return RedirectToAction(actionName:nameof(Index), controllerName:nameof(Category));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryAdminService.Get(id);

            if(category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Category category)
        {
            var dbCategory = await _categoryAdminService.Get(category.Id);

            if (string.IsNullOrEmpty(category.Name) || dbCategory.Name.Trim().ToLower() != category.Name.Trim().ToLower())
            {
                return Content("Cannot Delete");
            }

            if (dbCategory == null)
            {
                return NotFound();
            }

            await _categoryAdminService.Delete(category.Id);

            return RedirectToAction(actionName: nameof(Index), controllerName: nameof(Category));
        }

    }
}
 