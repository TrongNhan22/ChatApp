using ChatApp.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        public async Task Send(Message message)
        {
            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("broadcastMessage", message);
        }
    }
}
