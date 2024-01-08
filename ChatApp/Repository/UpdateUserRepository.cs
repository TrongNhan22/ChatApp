using ChatApp.Models;
using ChatApp.Interface;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ChatApp.Repository
{
    public class UpdateUserRepository : IUpdateUserRepository
    {
        public Task<User> GetUserById(string id)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(string id, string fullname, string gender, string birhtday, string email)
        {
            var filter = Builders<User>.Filter.Eq("id", ObjectId.Parse(id));
            var update = Builders<User>.Update
                .Set(u => u.fullname, fullname)
                .Set(u => u.email, email)
                .Set(u => u.gender, gender)
                .Set(u => u.birthday, birhtday);
        }
    }
}
