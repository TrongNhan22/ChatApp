using ChatApp.Models;
using ChatApp.Interface;
using MongoDB.Bson;
using MongoDB.Driver;
using ChatApp.Data;
using Microsoft.Extensions.Options;
using ChatApp.Models;
using ChatApp.Interface;

namespace ChatApp.Repository
{
    public class UpdateUserRepository : IUpdateUserRepository
    {
        private readonly IOptions<MongoDBSetting> _mongoDBSettting;
        private readonly IMongoCollection<User> _user;

        public UpdateUserRepository(IOptions<MongoDBSetting> mongoDBSettting)
        {
            _mongoDBSettting = mongoDBSettting;
            MongoClient mongoClient = new MongoClient(_mongoDBSettting.Value.ConnectionURI);
            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(_mongoDBSettting.Value.DatabaseName);
            _user = mongoDatabase.GetCollection<User>(_mongoDBSettting.Value.userCollectionName);
        }
        public async Task<User> GetUserById(string id)
        {
            var filter = Builders<User>.Filter.Eq("id", id);
            User userUpdate = new User();
            userUpdate = await _user.Find(filter).FirstOrDefaultAsync();
            return userUpdate;
        }

        public async Task<bool> UpdateUser(string id, string fullname, string gender, string birhtday, string email)
        {
            var filter = Builders<User>.Filter.Eq("id", ObjectId.Parse(id));
            var update = Builders<User>.Update
                .Set(u => u.fullname, fullname)
                .Set(u => u.email, email)
                .Set(u => u.gender, gender)
                .Set(u => u.birthday, birhtday);
            //User user = this.GetUserById(id);
            //var result = await user.UpdateOne(filter, update);
            var updateResult = await _user.UpdateOneAsync(filter, update);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}
