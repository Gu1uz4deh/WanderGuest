using Microsoft.AspNetCore.Mvc;
using System;
using WanderQuest.Application.Services.Public;

namespace WanderQuest.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly ISettingsQueryService _settingsService;

        public FooterViewComponent(ISettingsQueryService settingsService)
        {
            _settingsService = settingsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Dictionary<string, string> settings = await _settingsService.GetSettings();

            return View(settings);
        }
    }
}
