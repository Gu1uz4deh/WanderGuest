using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WanderQuest.Core.Entity;

namespace WanderQuest.Infrastructure.Models
{
    public class SliderImages : IEntity
    {
        public int Id { get; set; }
        public int SliderId { get; set; }
        public Slider Slider { get; set; }
        public int ImageId { get; set; }
        public Image Image { get; set; }
    }
}
