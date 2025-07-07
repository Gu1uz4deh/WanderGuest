using System;

namespace Services.Services
{
    public interface ISettingsService
    {
        public Task<Dictionary<string, string>> GetSettings();
    }
}
