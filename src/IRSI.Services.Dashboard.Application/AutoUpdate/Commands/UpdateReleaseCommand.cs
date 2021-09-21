using System;
using System.Threading;
using System.Threading.Tasks;
using IRSI.AutoUpdate.Contracts;
using IRSI.Services.Dashboard.Application.Common.Options;
using MassTransit;
using MassTransit.Definition;
using MediatR;
using Microsoft.Extensions.Options;

namespace IRSI.Services.Dashboard.Application.AutoUpdate.Commands
{
    public record UpdateReleaseCommand(string StoreId, string Owner, string RepositoryName, string InstallationPath,
        string ServiceExecutable, bool SetupStoreId, bool SetupServiceBus, bool SetupAzureConfiguration) : IRequest;

    public class UpdateReleaseCommandHandler : IRequestHandler<UpdateReleaseCommand>
    {
        private readonly IBus _bus;
        private readonly ServiceSettings _serviceSettings;

        public UpdateReleaseCommandHandler(IBus bus, IOptions<ServiceSettings> serviceOptions)
        {
            _bus = bus;
            _serviceSettings = serviceOptions.Value;
        }


        public async Task<Unit> Handle(UpdateReleaseCommand request, CancellationToken cancellationToken)
        {
            var nameFormatter = new DefaultEndpointNameFormatter(request.StoreId, false);
            var queueName = $"{request.StoreId}_{nameFormatter.Message<ReleaseAdded>()}";
            var endpoint = await _bus.GetSendEndpoint(new(new(_serviceSettings.AzureServiceBus.BaseUrl),
                new Uri(queueName, UriKind.Relative)));
            await endpoint.Send<ReleaseAdded>(new(request.Owner, request.RepositoryName, request.InstallationPath,
                request.ServiceExecutable, request.SetupStoreId, request.SetupServiceBus), cancellationToken);
            return Unit.Value;
        }
    }
}