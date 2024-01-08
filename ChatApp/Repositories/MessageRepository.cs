using ChatApp.Data;
using ChatApp.Interfaces;
using ChatApp.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

//https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-8.0&tabs=visual-studio
namespace ChatApp.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IMongoCollection<Message> _messageCollection;

        public MessageRepository(IOptions<MongoDBSetting> mongoDBSetting)
        {
            var mongoClient = new MongoClient(mongoDBSetting.Value.ConnectionURI);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSetting.Value.DatabaseName);

            _messageCollection = mongoDatabase.GetCollection<Message>(mongoDBSetting.Value.MessageCollectionName);
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
    }
}
