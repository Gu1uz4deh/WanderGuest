using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WanderQuest.Application.Implementations.Admin;
using WanderQuest.Application.Implementations.Public;
using WanderQuest.Application.Implementations.Public.BasketService;
using WanderQuest.Application.Implementations.Public.MessageServices;
using WanderQuest.Application.Services.Admin;
using WanderQuest.Application.Services.Public;
using WanderQuest.Application.Services.Public.BasketService;
using WanderQuest.Application.Services.Public.MessageServices;
using WanderQuest.BasketHandlers.Implementations;
using WanderQuest.BasketHandlers.Services;
using WanderQuest.Hubs;
using WanderQuest.Infrastructure.DAL;
using WanderQuest.Infrastructure.Models;

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
            services.AddScoped<ITeamMembersQueyService, TeamMembersQueryRepository>();
            services.AddScoped<IBasketDbService, BasketDbService>();
            services.AddScoped<IMessageDbService, MessageDbService>();

            services.AddScoped<IBasketItemService, BasketItemService>();
            services.AddScoped<IBasketSummaryService, BasketSummaryService>();


            services.AddControllersWithViews();
            services.AddSignalR(); // SignalR'ı aktif et
            services.AddSignalR().AddJsonProtocol(); //
            services.AddSingleton<IUserIdProvider, NameIdentifierProvider>(); // Bunu ekle

            services.AddHttpContextAccessor(); 



            services.AddIdentity<AppUser, IdentityRole>()
              .AddDefaultTokenProviders()
              .AddEntityFrameworkStores<AppDbContext>();
            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
            });

            services.AddDbContext<AppDbContext>(options => 
            {
                options.UseSqlServer(_configuration.GetConnectionString("Default"),
                    b => b.MigrationsAssembly("WanderQuest"));
            });



            services.AddAuthentication();
            services.AddAuthorization();

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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();


                app.UseEndpoints(endpoints =>
                {

                    endpoints.MapControllerRoute(
                        name: "ProductSearch",
                        pattern: "products/searchproduct/{title}",
                        defaults: new { controller = "Products", action = "SearchProduct" }
                    );
                    endpoints.MapControllerRoute
                    (
                        name: "Areas",
                        pattern: "{area:exists}/{controller=dashboard}/{action=index}/{id?}"
                    );
                    endpoints.MapControllerRoute
                    (
                        name: "default",
                        pattern: "{controller=home}/{action=index}/{id?}/{quantity?}"
                    );
                    endpoints.MapHub<ChatHub>("/chathub");
                });

            // 🔥 SignalR Hub route'u burada
        }
    }
}
