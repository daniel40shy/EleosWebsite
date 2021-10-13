using Microsoft.AspNetCore.Hosting;
using EleosWebApi.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using EleosWebApi.Models;
using System.IO;

namespace EleosWebApi
{
    public class Program
    {
        private static readonly string path = @$"{Environment.CurrentDirectory}\DownloadedRecords";

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            CreateLocalDownloadFileFolderIfNotExists();
            CreateDbIfNotExists(host);
            host.Run();
        }

        private static void CreateDbIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ZoomRecordingDbContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }

        private static void CreateLocalDownloadFileFolderIfNotExists()
        {
            Directory.CreateDirectory(path);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
