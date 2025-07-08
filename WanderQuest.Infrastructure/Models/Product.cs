using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WanderQuest.Infrastructure.Models
{
    public class Product : BaseEntity
    {
        //[Required(ErrorMessage = "Please enter the title")]
        //[MaxLength(255, ErrorMessage = "Title length max 255 length")]
        public string Title { get; set; }
        //[Range(0, 25000, ErrorMessage = "Please enter under the 25000 and upper 0")]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ProductImages> ProductImages { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
