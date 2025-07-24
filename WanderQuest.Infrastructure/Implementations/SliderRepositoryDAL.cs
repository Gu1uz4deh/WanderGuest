using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WanderQuest.Core.EFRepository.EFEntityRepositoryBase;
using WanderQuest.Infrastructure.Abstracts;
using WanderQuest.Infrastructure.DAL;
using WanderQuest.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace WanderQuest.Infrastructure.Implementations
{
    public class SliderRepositoryDAL : EFEntityRepositoryBase<Slider, AppDbContext>, ISliderDAL
    {   
        public SliderRepositoryDAL(AppDbContext context) : base (context) {  }
        
        public async Task<List<Slider>> GetAllSlidersWithImagesAsync()
        {
            return await _context.Sliders
                .Where(s => !s.IsDeleted)
                .Include(s => s.SliderImages)
                .ThenInclude(si => si.Image)
                .ToListAsync();
        }
        public async Task<Slider> GetSliderWithImagesAsync(int id)
        {
            return await _context.Sliders
                .Where(s => !s.IsDeleted && s.Id == id)
                .Include(s => s.SliderImages)
                .ThenInclude(si => si.Image)
                .FirstOrDefaultAsync();
        }
        public async Task<List<Slider>> GetPagedSlidersWithImagesAsync(int skip = 0, int take = 4)
        {
            return await _context.Sliders
                .Where(s => !s.IsDeleted)
                .Include(s => s.SliderImages)
                .ThenInclude(si => si.Image)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }
    }
}
