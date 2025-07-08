using System;

namespace WanderQuest.Application.Services.Public
{
    public interface ISettingsQueryService
    {
        public Task<Dictionary<string, string>> GetSettings();
    }
}
