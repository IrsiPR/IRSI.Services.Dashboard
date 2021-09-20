using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace IRSI.Services.Dashboard.Application.AutoUpdate.Notifications
{
    public record ReleaseUpdatedNotification : INotification;
    
    public class ReleaseUpdatedNotificationHandler: INotificationHandler<ReleaseUpdatedNotification>
    {
        public Task Handle(ReleaseUpdatedNotification notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}