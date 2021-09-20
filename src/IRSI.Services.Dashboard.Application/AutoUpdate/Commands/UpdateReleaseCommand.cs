using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace IRSI.Services.Dashboard.Application.AutoUpdate.Commands
{
    public record UpdateReleaseCommand : IRequest;
    
    public class UpdateReleaseCommandHandler : IRequestHandler<UpdateReleaseCommand>
    {
        public Task<Unit> Handle(UpdateReleaseCommand request, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }
    }
}