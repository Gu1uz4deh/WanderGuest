using WanderQuest.Application.Implementations.Admin;
using WanderQuest.Application.Implementations.Public;
using WanderQuest.Application.Services.Admin;
using WanderQuest.Application.Services.Public;
using WanderQuest.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace WanderQuest
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession(n =>
            {
                n.IdleTimeout = TimeSpan.FromSeconds(10);
            });


            services.AddScoped<ISettingsQueryService, SettingsQueryRepository>();
            services.AddScoped<ICategoryAdminService, CategoryAdminRepository>();
            services.AddScoped<IProductsQueryService, ProductsQueryRepository>();
            services.AddScoped<ISlidersQueryService, SlidersQueryRepository>();
            services.AddScoped<ICategoriesQueryService, CategoriesQueryRepository>();


            services.AddControllersWithViews();
             
            services.AddDbContext<AppDbContext>(options => 
            {
                options.UseSqlServer(_configuration.GetConnectionString("Default"),
                    b => b.MigrationsAssembly("WanderQuest"));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession(new SessionOptions()
            {
                IdleTimeout = TimeSpan.FromSeconds(5),
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute
                (
                    name: "Areas",
                    pattern: "{area:exists}/{controller=dashboard}/{action=index}/{id?}"
                );
                endpoints.MapControllerRoute
                (
                    name: "default",
                    pattern: "{controller=home}/{action=index}/{id?}"
                );
                
            });
        }
    }
}
