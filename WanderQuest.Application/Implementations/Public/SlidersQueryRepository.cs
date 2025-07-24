using Microsoft.EntityFrameworkCore;
using System;
using WanderQuest.Application.Services.Public;
using WanderQuest.Infrastructure.Abstracts;
using WanderQuest.Infrastructure.Implementations;
using WanderQuest.Infrastructure.Models;

namespace WanderQuest.Application.Implementations.Public
{
    public class SlidersQueryRepository : ISlidersQueryService
    {
        private readonly ISliderDAL _repository;
        public SlidersQueryRepository(ISliderDAL repository)
        {
            _repository = repository;
        }
        public async Task<List<Slider>> GetAll()
        {
            //var sliders = await _context.Sliders.Where(n => !n.IsDeleted)
            //                                 .Include(n => n.SliderImages)
            //                                 .ThenInclude(n => n.Image)
            //                                 .ToListAsync();
            var sliders = await _repository.GetAllAsync(n => !n.IsDeleted);
            return sliders;
        }

        public async Task<Slider> GetById(int id)
        {
            //var slider = await _context.Sliders.Where(n => !n.IsDeleted && n.Id == id)
            //                                 .Include(n => n.SliderImages)
            //                                 .ThenInclude(n => n.Image)
            //                                 .FirstOrDefaultAsync();
            var slider = await _repository.GetAsync(n => !n.IsDeleted && n.Id == id);
            return slider;
        }

        public async Task<List<Slider>> GetPaged(int skip = 0, int take = 4)
        {
            //var sliders = await _context.Sliders.Where(n => !n.IsDeleted)
            //                                 .Include(n => n.SliderImages)
            //                                 .ThenInclude(n => n.Image)
            //                                 .Skip(skip)
            //                                 .Take(take)
            //                                 .ToListAsync();
            var sliders = await _repository.GetAllAsync(n => !n.IsDeleted);
            return sliders;
        }
    }
}
