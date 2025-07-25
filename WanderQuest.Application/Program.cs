using Microsoft.AspNetCore.Hosting;
using System;
using WanderQuest.Application;

namespace WanderQuest.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
            CreateHostBuilder(args, port).Build().Run();
            //CreateHostBuilder(args).Build().Run();
        }
        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //    .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        //}
        public static IHostBuilder CreateHostBuilder(string[] args, string port) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseUrls($"http://*:{port}");
                });
    }
}



















//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

//app.Run();
