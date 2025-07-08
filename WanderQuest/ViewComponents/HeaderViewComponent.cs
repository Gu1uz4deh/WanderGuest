using Microsoft.AspNetCore.Mvc;
using System;
using WanderQuest.Application.Services.Public;

namespace WanderQuest.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly ISettingsQueryService _settingsService;
        public HeaderViewComponent(ISettingsQueryService settingsService)
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
