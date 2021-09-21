namespace IRSI.Services.Dashboard.Notifications.Abstractions
{
    public interface INotification
    {
        NotificationType NotificationType { get; set; }
    }
}