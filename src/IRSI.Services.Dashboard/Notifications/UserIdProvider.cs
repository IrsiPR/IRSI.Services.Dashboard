#nullable enable
using Microsoft.AspNetCore.SignalR;

namespace IRSI.Services.Dashboard.Notifications
{
    public class UserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Identity?.Name;
        }
    }
}