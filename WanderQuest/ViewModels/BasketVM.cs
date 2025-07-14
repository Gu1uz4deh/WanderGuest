using System;

namespace WanderQuest.ViewModels
{
    public class BasketVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public int Count { get; set; }

    }
}
