using WanderQuest.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WanderQuest.Infrastructure.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<SliderImages> SliderImages { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<TeamMemberImages> TeamMemberImages { get; set; }
    }
}
