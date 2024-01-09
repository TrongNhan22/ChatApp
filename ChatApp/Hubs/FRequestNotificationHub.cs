using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    public class FRequestNotificationHub : Hub
    {
        public async Task SendNotification(string userToNotifyId, string message)
        {
            await Clients.User(userToNotifyId).SendAsync("ReceiveNotification", message);
        }
    }
}
