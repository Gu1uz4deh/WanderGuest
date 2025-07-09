using Microsoft.EntityFrameworkCore;
using System;
using WanderQuest.Application.Services.Public;
using WanderQuest.Infrastructure.DAL;
using WanderQuest.Infrastructure.Models;

namespace WanderQuest.Application.Implementations.Public
{
    public class SlidersQueryRepository : ISlidersQueryService
    {
        private readonly AppDbContext _context;
        public SlidersQueryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Slider>> GetAll()
        {
            var sliders = await _context.Sliders.Where(n => !n.IsDeleted)
                                             .Include(n => n.SliderImages)
                                             .ThenInclude(n => n.Image)
                                             .ToListAsync();
            return sliders;
        }

        public async Task<Slider> GetById(int id)
        {
            var slider = await _context.Sliders.Where(n => !n.IsDeleted && n.Id == id)
                                             .Include(n => n.SliderImages)
                                             .ThenInclude(n => n.Image)
                                             .FirstOrDefaultAsync();
            return slider;
        }

        public async Task<List<Slider>> GetPaged(int skip = 0, int take = 4)
        {
            var sliders = await _context.Sliders.Where(n => !n.IsDeleted)
                                             .Include(n => n.SliderImages)
                                             .ThenInclude(n => n.Image)
                                             .Skip(skip)
                                             .Take(take)
                                             .ToListAsync();
            return sliders;
        }
    }
}
