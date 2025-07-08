using WanderQuest.Infrastructure.DAL;
using WanderQuest.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WanderQuest.Application.Services.Public;

namespace WanderQuest.Application.Implementations.Public
{
    public class SettingsQueryRepository : ISettingsQueryService
    {
        private readonly AppDbContext _context;
        public SettingsQueryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, string>> GetSettings()
        {
            var data = await _context.Settings.ToDictionaryAsync(n => n.Key, n => n.Value);

            return data;
        }
    }
}
