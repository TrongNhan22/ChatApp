using ChatApp.Interface;
using ChatApp.Models;
using Microsoft.AspNetCore.SignalR;
using SignalRChatDemo.Models;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly static ConnectionMapping <string> _connections = new();
        private readonly IMessageRepository messageRepository;

        public ChatHub(IMessageRepository messageRepository)
        {
            this.messageRepository = messageRepository;
        }
        public async Task Send(Message message)
        {
            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("broadcastMessage", message);
        }
        public async Task SendPrivate(Message message)
        {
            await messageRepository.CreateAsync(message);
            //Then SendAsync to all ClientID of that connectionId
            await Clients.All.SendAsync("receivePrivate", message);
        }
        public override async Task OnConnectedAsync()
        {
            var userList = _connections.GetUsers();
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveCurrentUserList", _connections.GetUsers());

            string connectionId = Context.User?.Identity?.Name ?? Context.ConnectionId;
            _connections.Add(connectionId, connectionId);

            //Broadcast a message to all chat announcing a user has joined
            var broadcastMessage = new Message
            {
                Id = "",
                SenderId = "SYSTEM",
                ReceiverId = "",
                Content = $"{connectionId} đã tham gia chat",
            };
            await Send(broadcastMessage);
            await Clients.All.SendAsync("UserJoined", connectionId);
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string connectionId = Context.User?.Identity?.Name ?? Context.ConnectionId;
            _connections.Remove(connectionId, connectionId);

            //Broadcast a message to all chat announcing a user has joined
            var broadcastMessage = new Message
            {
                Id = "",
                SenderId = "SYSTEM",
                ReceiverId = "",
                Content = $"{connectionId} đã rời khỏi chat",
            };
            await Send(broadcastMessage);
            await Clients.All.SendAsync("UserLeft", connectionId);
        }
    }
}
