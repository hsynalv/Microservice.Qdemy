using FreeCourse.Services.Catalog.Service.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scoped = host.Services.CreateScope())
            {
                var serviceProvider = scoped.ServiceProvider;
                var categoryService = serviceProvider.GetRequiredService<ICategoryService>();

                if (!categoryService.GetAllAsync().Result.Data.Any())
                {
                    categoryService.CreateAsync(new DTOs.CategoryDto { Name = "Psikoloji" }).Wait();
                    categoryService.CreateAsync(new DTOs.CategoryDto { Name = "Borsa" }).Wait();
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
