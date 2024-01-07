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
        private readonly IOptions<MongoDBSetting> _mongoDBSettting;
        private readonly IMongoCollection<User> user;

        public SearchFriendRepository(IOptions<MongoDBSetting> mongoDBSettting)
        {
            _mongoDBSettting = mongoDBSettting;
            MongoClient mongoClient = new MongoClient(_mongoDBSettting.Value.ConnectionURI);
            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(_mongoDBSettting.Value.DatabaseName);
            user = mongoDatabase.GetCollection<User>(_mongoDBSettting.Value.userCollectionName);
        }


        public async Task<List<User>> GetUserByNameAsync(string userName)
        {
            var filter = Builders<User>.Filter.Regex("name", new MongoDB.Bson.BsonRegularExpression(userName, "i")); // Case-insensitive search on 'name' field
            return await user.Find(filter).ToListAsync();
        }

    }
}