using System.Reflection;
using IRSI.Services.Dashboard.Application.Common.Options;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IRSI.Services.Dashboard.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServiceSettings>(settings =>
                configuration.GetSection(nameof(ServiceSettings)).Bind(settings));

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddMassTransit(configurator =>
            {
                configurator.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>().AzureServiceBus
                        .ConnectionString);
                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddMassTransitHostedService();
        }
    }
}