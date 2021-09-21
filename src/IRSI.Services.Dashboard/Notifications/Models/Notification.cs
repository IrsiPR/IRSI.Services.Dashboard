using IRSI.Services.Dashboard.Notifications.Abstractions;

namespace IRSI.Services.Dashboard.Notifications.Models
{
    public class Notification<T> : INotification
    {
        public NotificationType NotificationType { get; set; }
        public T Payload { get; set; }
    }
}