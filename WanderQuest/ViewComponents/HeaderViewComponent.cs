using Microsoft.AspNetCore.Mvc;
using Services.Services;
using System;

namespace WanderQuest.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly ISettingsService _settingsService;
        public HeaderViewComponent(ISettingsService settingsService)
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
