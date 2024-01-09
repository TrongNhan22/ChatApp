using AddFriend.Models;
using ChatApp.Data;
using ChatApp.Interface;
using ChatApp.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChatApp.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IOptions<MongoDBSetting> _mongoDB;
        private readonly IMongoCollection<User> _user;
        private readonly IMongoCollection<FriendRequest> _friendRequests;
        private readonly IMongoCollection<Relationship> _relationship;

        public NotificationRepository(IOptions<MongoDBSetting> mongoDB)
        {
            _mongoDB = mongoDB;
            MongoClient client = new MongoClient(_mongoDB.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(_mongoDB.Value.DatabaseName);
            _user = database.GetCollection<User>(_mongoDB.Value.userCollectionName);
            _friendRequests = database.GetCollection<FriendRequest>(_mongoDB.Value.friendRequestCollectionName);
            _relationship = database.GetCollection<Relationship>(_mongoDB.Value.relationshipCollectionName);
        }

        public async Task<User> GetUserAsync(string id)
        {
            var filter = Builders<User>.Filter.Eq("id", id);
            User user = new User();
            user = await _user.Find(filter).FirstOrDefaultAsync();
            return user;
        }

        public async Task<List<FriendRequest>> GetListRequestAsync(string receiverId)
        {
            var filter = Builders<FriendRequest>.Filter.Eq("receiverId", receiverId);

            List<FriendRequest> fr = await _friendRequests.Find(filter).ToListAsync();

            if (fr == null)
            {
                fr = new List<FriendRequest>();
            }

            return fr;
        }

        public async Task<bool> CancelRequestAsync(string makerId, string receiverId)
        {
            var filter = Builders<FriendRequest>.Filter.Eq("makerId", makerId) &
                Builders<FriendRequest>.Filter.Eq("receiverId", receiverId);

            var filter1 = Builders<FriendRequest>.Filter.Eq("makerId", receiverId) &
                Builders<FriendRequest>.Filter.Eq("receiverId", makerId);

            var deleteResult = await _friendRequests.DeleteOneAsync(filter);
            var deleteResult1 = await _friendRequests.DeleteOneAsync(filter1);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0 || deleteResult1.IsAcknowledged && deleteResult1.DeletedCount > 0;
        }

        public async Task<bool> AcceptRequestAsync(string makerId, string receiverId)
        {
            var filter1 = Builders<Relationship>.Filter.Eq("userId", makerId) &
                Builders<Relationship>.Filter.Eq("friendId", receiverId);
            var filter2 = Builders<Relationship>.Filter.Eq("friendId", makerId) &
                Builders<Relationship>.Filter.Eq("userId", receiverId);

            Relationship rl1 = _relationship.Find(filter1).FirstOrDefault();
            Relationship rl2 = _relationship.Find(filter2).FirstOrDefault();
            var relationship1 = new Relationship() { userId = makerId, friendId = receiverId };
            var relationship2 = new Relationship() { friendId = makerId, userId = receiverId };

            if (rl1 != null && rl2 != null)
            {
                return false;
            }
            else
            {
                try
                {
                    await _relationship.InsertOneAsync(relationship1);
                    await _relationship.InsertOneAsync(relationship2);
                    var rs = await CancelRequestAsync(receiverId, makerId);
                    return true && rs; // Trả về true nếu không có ngoại lệ nào xảy ra
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex); // Ghi lại lỗi
                    return false; // Trả về false nếu có lỗi
                }
            }
        }
    }
}
