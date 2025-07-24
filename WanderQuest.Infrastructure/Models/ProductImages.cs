using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WanderQuest.Core.Entity;

namespace WanderQuest.Infrastructure.Models
{
    public class ProductImages : IEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ImageId { get; set; }
        public Product Product { get; set; }
        public Image Image { get; set; }
    }
}
