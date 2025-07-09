using Microsoft.AspNetCore.Mvc;
using System;
using WanderQuest.Application.Services.Public;

namespace WanderQuest.ViewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly ICategoriesQueryService _categoryQueryService;
        public CategoriesViewComponent(ICategoriesQueryService categoryQueryService)
        {
            _categoryQueryService = categoryQueryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryQueryService.GetAll();
            return View(categories);
        }
    }
}
