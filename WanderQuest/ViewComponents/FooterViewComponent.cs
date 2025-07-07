using Microsoft.AspNetCore.Mvc;
using Services.Services;
using System;

namespace WanderQuest.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly ISettingsService _settingsService;

        public FooterViewComponent(ISettingsService settingsService)
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
