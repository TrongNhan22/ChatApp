using ChatApp.Controllers;
using ChatApp.Data;
using ChatApp.Interface;
using ChatApp.Models;
using ChatApp.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChatApp.Repository
{
    public class LoginRepository : ILogin
    {
        private readonly IOptions<MongoDBSetting> _mongoDBSettting;
        private readonly IMongoCollection<User> _user;
        //User user_login = Globals.user;
        public LoginRepository(IOptions<MongoDBSetting> mongoDBSettting)
        {
            _mongoDBSettting = mongoDBSettting;
            MongoClient mongoClient = new MongoClient(_mongoDBSettting.Value.ConnectionURI);
            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(_mongoDBSettting.Value.DatabaseName);
            _user = mongoDatabase.GetCollection<User>(_mongoDBSettting.Value.userCollectionName);
        }

        public async Task<bool> GetAccountAsync(User user)
        {
            var filter = Builders<User>.Filter.And(
                Builders<User>.Filter.Eq(u => u.email, user.email),
                Builders<User>.Filter.Eq(u => u.password, user.password)
            );

            var result = await _user.Find(filter).FirstOrDefaultAsync();
            return result != null;
        }

        public async Task<User> GetUser(User user)
        {
            var filter = Builders<User>.Filter.And(
                Builders<User>.Filter.Eq(u => u.email, user.email),
                Builders<User>.Filter.Eq(u => u.password, user.password)
            );

            User user_login = await _user.Find(filter).FirstOrDefaultAsync();
            return user_login;
        }

    }
}

