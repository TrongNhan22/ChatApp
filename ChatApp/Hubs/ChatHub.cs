using ChatApp.Controllers;
using ChatApp.Interface;
using ChatApp.Models;
using ChatApp.ViewModels;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using SignalRChatDemo.Models;
using System.Globalization;

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

        ///This function is only for testing
        public async Task Send(Message message)
        {
            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("broadcastMessage", message);
        }
        public async Task SendPrivate(Message message)
        {
            DateTime.Now.ToString("F");
            message.Date = DateTime.Now;
            await messageRepository.CreateAsync(message);
            //Then SendAsync to all ClientID of that connectionId
            var messageViewModel = new MessageViewModel
            {
                SenderId = message.SenderId,
                Content = message.Content,
                Media = message.Media,
                Date = message.Date.ToString("g")
            };
            //Send message to all receiver connections
            foreach (var connection in _connections.GetConnections(message.ReceiverId))
            {
                await Clients.Client(connection).SendAsync("ReceivePrivate", messageViewModel);
            }
            //Send message to all sender connections
            foreach (var connection in _connections.GetConnections(message.SenderId))
            {
                await Clients.Client(connection).SendAsync("ReceivePrivate", messageViewModel);
            }
        }
        public override async Task OnConnectedAsync()
        {
            //var userList = _connections.GetUsers();
            //await Clients.Client(Context.ConnectionId).SendAsync("ReceiveCurrentUserList", _connections.GetUsers());

            string connectionId = Context.ConnectionId;
            if (Globals.user_login == null)
                return;
            _connections.Add(Globals.user_login.id!, connectionId);

            //Broadcast a message to all chat announcing a user has joined
            //var broadcastMessage = new Message
            //{
            //    Id = "",
            //    SenderId = "SYSTEM",
            //    ReceiverId = "",
            //    Content = $"{connectionId} đã tham gia chat",
            //};
            //await Send(broadcastMessage);
            //await Clients.All.SendAsync("UserJoined", connectionId);
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string connectionId = Context.ConnectionId;
            if (Globals.user_login == null) return;
            _connections.Remove(Globals.user_login.id!, connectionId);

            //Broadcast a message to all chat announcing a user has joined
            //var broadcastMessage = new Message
            //{
            //    Id = "",
            //    SenderId = "SYSTEM",
            //    ReceiverId = "",
            //    Content = $"{connectionId} đã rời khỏi chat",
            //};
            //await Send(broadcastMessage);
            //await Clients.All.SendAsync("UserLeft", connectionId);
        }
    }
}
