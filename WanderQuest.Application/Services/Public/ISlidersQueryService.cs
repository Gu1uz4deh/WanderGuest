using System;
using WanderQuest.Infrastructure.Models;

namespace WanderQuest.Application.Services.Public
{
    public interface ISlidersQueryService
    {
        Task<List<Slider>> GetAll();
        Task<Slider> GetById(int id);
        Task<List<Slider>> GetPaged(int skip = 0, int take = 4);
    }
}
