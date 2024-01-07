using ChatApp.Data;
using ChatApp.Interface;
using ChatApp.Models;
using ChatApp.ViewModels;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;

//https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-8.0&tabs=visual-studio
namespace ChatApp.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IMongoCollection<Message> _messageCollection;
        private readonly IMongoCollection<User> _userCollection;
        public MessageRepository(IOptions<MongoDBSetting> mongoDBSetting)
        {
            var mongoClient = new MongoClient(mongoDBSetting.Value.ConnectionURI);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSetting.Value.DatabaseName);

            _messageCollection = mongoDatabase.GetCollection<Message>(mongoDBSetting.Value.MessageCollectionName);
            _userCollection = mongoDatabase.GetCollection<User>(mongoDBSetting.Value.userCollectionName);
        }
        public async Task<IEnumerable<Message>> GetListAsync() =>
            await _messageCollection.Find(_ => true).ToListAsync();
        public async Task<IEnumerable<Message>> GetListBySenderAndReceiverIdAsync(string senderId, string receiverId) =>
            await _messageCollection.Find(
                x => x.SenderId == senderId && x.ReceiverId == receiverId).ToListAsync();
        public async Task<Message> GetMessageByIdAsync(string id) =>
            await _messageCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task CreateAsync(Message newMessage) =>
            await _messageCollection.InsertOneAsync(newMessage);
        public async Task UpdateAsync(string id, Message updatedMessage) =>
            await _messageCollection.ReplaceOneAsync(x => x.Id == id, updatedMessage);
        public async Task DeleteAsync(string id) =>
            await _messageCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<IEnumerable<ChatPartnerViewModel>> GetChatPartner()
        {
            //for testing
            string thisUser = "65340b20b32df212d36f15ad";
            var chatPartnerList = await _messageCollection.Find(x => x.SenderId == thisUser || x.ReceiverId == thisUser)
                .Project(x => x.SenderId == thisUser ?
                new ChatPartnerViewModel
                {
                    id = x.ReceiverId,
                    lastInteractionTime = x.Date,
                    latestMessage = string.IsNullOrEmpty(x.Content) ? "Hình ảnh" : x.Content
                } :
                new ChatPartnerViewModel
                {
                    id = x.SenderId,
                    lastInteractionTime = x.Date,
                    latestMessage = string.IsNullOrEmpty(x.Content) ? "Hình ảnh" : x.Content
                })
                .ToListAsync();
            for (int i = 0; i < chatPartnerList.Count; i++)
            {
                var user = await _userCollection.Find(x=> x.id== chatPartnerList[i].id).FirstOrDefaultAsync();
                chatPartnerList[i].avatar = user.avatar;
                chatPartnerList[i].fullname = user.fullname;
            }
            return chatPartnerList;
        }
     
    }
}
