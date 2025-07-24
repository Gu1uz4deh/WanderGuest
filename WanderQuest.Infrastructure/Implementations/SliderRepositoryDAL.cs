using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WanderQuest.Core.EFRepository.EFEntityRepositoryBase;
using WanderQuest.Infrastructure.Abstracts;
using WanderQuest.Infrastructure.DAL;
using WanderQuest.Infrastructure.Models;

namespace WanderQuest.Infrastructure.Implementations
{
    public class SliderRepositoryDAL : EFEntityRepositoryBase<Slider, AppDbContext>, ISliderDAL
    {
        public SliderRepositoryDAL(AppDbContext context) : base (context) { }
    }
}
