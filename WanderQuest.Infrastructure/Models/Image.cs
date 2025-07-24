using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WanderQuest.Core.Entity;

namespace WanderQuest.Infrastructure.Models
{
    public class Image : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductImages> ProductImages { get; set; }
        public List<SliderImages> SliderImages { get; set; }
        public List<TeamMemberImages> TeamMemberImages { get; set; }

    }
}
