using System;
using WanderQuest.Core.Entity;

namespace WanderQuest.Infrastructure.Models
{
    public class Slider : BaseEntity, IEntity
    {
        //public string ImgUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<SliderImages> SliderImages { get; set; }
    }
}
