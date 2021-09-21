using System;
using System.Threading;
using System.Threading.Tasks;
using IRSI.Polling.Contracts;
using IRSI.Polling.Contracts.Models;
using IRSI.Services.Dashboard.Application.Common.Options;
using MassTransit;
using MassTransit.Definition;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IRSI.Services.Dashboard.Application.Polling.Commands
{
    public record GeneratePollingCommand(string StoreId, DateSelectionType DateSelectionType, DateTime StartDate,
        DateTime? EndDate) : IRequest;

    public class GeneratePollingCommandHandler : IRequestHandler<GeneratePollingCommand>
    {
        private readonly IBus _bus;
        private readonly IClientFactory _clientFactory;
        private readonly ServiceSettings _serviceSettings;
        private readonly ILogger<GeneratePollingCommandHandler> _logger;

        public GeneratePollingCommandHandler(IClientFactory clientFactory, IBus bus,
            IOptions<ServiceSettings> serviceOptions, ILogger<GeneratePollingCommandHandler> logger)
        {
            _clientFactory = clientFactory;
            _bus = bus;
            _logger = logger;
            _serviceSettings = serviceOptions.Value;
        }

        public async Task<Unit> Handle(GeneratePollingCommand request, CancellationToken cancellationToken)
        {
            var nameFormatter = new DefaultEndpointNameFormatter(request.StoreId, false);
            var queueName = $"{request.StoreId}_{nameFormatter.Message<GeneratePolling>()}";

            var serviceAddress = new Uri(
                new(_serviceSettings.AzureServiceBus.BaseUrl),
                new Uri(queueName, UriKind.Relative)
            );
            var client = _clientFactory.CreateRequestClient<GeneratePolling>(serviceAddress, TimeSpan.FromMinutes(5));

            // var endpoint = await _bus.GetSendEndpoint(new(
            //     new(_options.Value.AzureServiceBus.BaseUrl),
            //     new Uri(queueName, UriKind.Relative)
            // ));

            // await endpoint.Send<GeneratePolling>(
            //     new(request.DateSelectionType,
            //         request.StartDate.Date,
            //         request.EndDate?.Date),
            //     cancellationToken);

            try
            {
                var response =
                    await client
                        .GetResponse<GeneratePollingSuccess, GeneratePollingFailed>(new(request.DateSelectionType,
                            request.StartDate.Date,
                            request.EndDate?.Date), cancellationToken);
            }
            catch (Exception ex)
            {
                
            }

            return Unit.Value;
        }
    }
}