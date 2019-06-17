using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetcoreOnlineShop.Data.EF;

namespace NetCoreOnlineShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHost host = CreateWebHostBuilder(args).Build();
            //using (IServiceScope scope = host.Services.CreateScope())
            //{
            //    IServiceProvider services = scope.ServiceProvider;
            //    try
            //    {
            //        var dbInitializer = services.GetService<DbInitializer>();
            //        dbInitializer.Seed().Wait();
            //    }
            //    catch (Exception ex)
            //    {
            //        var logger = services.GetService<ILogger<Program>>();
            //        logger.LogError(ex, "Error seeding database");
            //    }
            //}

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                .UseStartup<Startup>();
    }
}
