using Data.DAL;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Services;
using System;
using System.Linq;

namespace Services.Implementations
{
    public class SettingsServices : ISettingsService
    {
        private readonly AppDbContext _context;
        public SettingsServices(AppDbContext context)
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
