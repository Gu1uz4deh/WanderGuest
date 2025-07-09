using Microsoft.AspNetCore.Mvc;
using System;
using WanderQuest.Application.Services.Public;

namespace WanderQuest.ViewComponents
{
    public class SlidersViewComponent : ViewComponent
    {
        private readonly ISlidersQueryService _sliderQueryService;
        public SlidersViewComponent(ISlidersQueryService sliderQueryService)
        {
            _sliderQueryService = sliderQueryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sliders = await _sliderQueryService.GetPaged();
            return View(sliders);
        }
    }
}
