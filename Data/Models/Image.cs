using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductImages> ProductImages { get; set; }
    }
}
