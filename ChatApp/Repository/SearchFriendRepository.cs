using ChatApp.Data;
using ChatApp.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChatApp.Repository
{
    public class SearchFriendRepository : ISearchFriendRepository
    {
        private readonly IOptions<MongoDBSetting> mongoDBSettting;
        private readonly IMongoCollection<User> user;

        public SearchFriendRepository(IOptions<MongoDBSetting> mongoDBSettting)
        {
            this.mongoDBSettting = mongoDBSettting;
            user = this.mongoDBSettting.Value.userCollection;
        }

        public async Task<List<User>> GetUserByNameAsync(string userName)
        {
            var filter = Builders<User>.Filter.Regex("name", new MongoDB.Bson.BsonRegularExpression(userName, "i")); // Case-insensitive search on 'name' field
            return await user.Find(filter).ToListAsync();
        }

    }
}