using System;

namespace WanderQuest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var builder = WebApplication.CreateBuilder(args);

            //// Startup sınıfını elle başlat
            //var startup = new Startup();
            //startup.ConfigureServices(builder.Services);

            //var app = builder.Build();

            //startup.Configure(app, app.Environment);

            //app.Run();

            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
