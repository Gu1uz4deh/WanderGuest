using Data.Models;
using System;

namespace WanderQuest.ViewModel
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
    }
}
