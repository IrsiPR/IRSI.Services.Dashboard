using System;
using System.Threading;
using System.Threading.Tasks;
using IRSI.Polling.Contracts;
using IRSI.Services.Dashboard.Application.Common.Options;
using MassTransit;
using MassTransit.Definition;
using MediatR;
using Microsoft.Extensions.Options;

namespace IRSI.Services.Dashboard.Application.Polling.Commands
{
    public record GeneratePollingCommand(string StoreId, DateSelectionType DateSelectionType, DateTime StartDate,
        DateTime? EndDate) : IRequest;

    public class GeneratePollingCommandHandler : IRequestHandler<GeneratePollingCommand>
    {
        private readonly IBus _bus;
        private IOptions<ServiceSettings> _options;

        public async Task<Unit> Handle(GeneratePollingCommand request, CancellationToken cancellationToken)
        {
            var nameFormatter = new DefaultEndpointNameFormatter(request.StoreId, false);
            var queueName = $"{request.StoreId}_{nameFormatter.Message<GeneratePolling>()}";
            var endpoint = await _bus.GetSendEndpoint(new(
                new(_options.Value.AzureServiceBus.BaseUrl),
                new Uri(queueName, UriKind.Relative)
            ));

            await endpoint.Send<GeneratePolling>(
                new(request.DateSelectionType,
                    request.StartDate.Date,
                    request.EndDate?.Date),
                cancellationToken);
            return Unit.Value;
        }
    }
}