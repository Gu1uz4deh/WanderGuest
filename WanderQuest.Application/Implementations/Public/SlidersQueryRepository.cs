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
            var sliders = await _repository.GetAllSlidersWithImagesAsync();
            return sliders;
        }

        public async Task<Slider> GetById(int id)
        {
            var slider = await _repository.GetSliderWithImagesAsync(id);
            return slider;
        }

        public async Task<List<Slider>> GetPaged(int skip = 0, int take = 4)
        {
            var sliders = await _repository.GetPagedSlidersWithImagesAsync(skip, take);
            return sliders;
        }
    }
}
