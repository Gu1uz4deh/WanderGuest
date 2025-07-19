using System;

namespace WanderQuest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Startup.cs'teki class'ı kullanmak için
            var startup = new Startup(builder.Configuration);

            // servisleri ekle
            startup.ConfigureServices(builder.Services);

            var app = builder.Build();

            // middlewareleri uygula
            startup.Configure(app, builder.Environment);

            app.Run();
            //CreateHostBuilder(args).Build().Run();
        }
        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //    .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
