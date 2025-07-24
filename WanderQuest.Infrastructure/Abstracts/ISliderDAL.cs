using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WanderQuest.Core.EFRepository;
using WanderQuest.Infrastructure.Models;

namespace WanderQuest.Infrastructure.Abstracts
{
    public interface ISliderDAL : IRepositoryBase<Slider>
    {
        Task<List<Slider>> GetAllSlidersWithImagesAsync();
        Task<Slider> GetSliderWithImagesAsync(int id);
        Task<List<Slider>> GetPagedSlidersWithImagesAsync(int skip, int take);
    }
}
