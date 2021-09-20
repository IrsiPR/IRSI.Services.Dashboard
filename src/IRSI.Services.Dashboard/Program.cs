using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace IRSI.Services.Dashboard
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Console.Title = "IRSI.Services.Dashboard";

            var host = CreateHostBuilder(args).Build();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            try
            {
                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Backend Terminated Unexpectedly");
                return 1;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, configuration) =>
                {
                    if (context.HostingEnvironment.IsProduction())
                        configuration.MinimumLevel.Information();
                    else
                        configuration.MinimumLevel.Debug();

                    configuration.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .Enrich.WithProperty("Application", "IRSI.Services.Api")
                        .WriteTo.Console();
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}