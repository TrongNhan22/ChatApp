using AddFriend.Models;
using ChatApp.Data;
using ChatApp.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ChatApp.Repository
{
    public class SearchFriendRepository : ISearchFriendRepository
    {
        private readonly IOptions<MongoDBSetting> _mongoDBSettting;
        private readonly IMongoCollection<User> _user;
        private readonly IMongoCollection<Relationship> _relationship;
        private readonly IMongoCollection<FriendRequest> _friendRequests;

        public SearchFriendRepository(IOptions<MongoDBSetting> mongoDBSettting)
        {
            _mongoDBSettting = mongoDBSettting;
            MongoClient client = new MongoClient(_mongoDBSettting.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(_mongoDBSettting.Value.DatabaseName);
            _user = database.GetCollection<User>(_mongoDBSettting.Value.userCollectionName);
            _relationship = database.GetCollection<Relationship>(_mongoDBSettting.Value.relationshipCollectionName);
            _friendRequests = database.GetCollection<FriendRequest>(_mongoDBSettting.Value.friendRequestCollectionName);
        }


        public async Task<List<User>> GetUserByNameAsync(string userName)
        {
            var filter = Builders<User>.Filter.Regex("fullname", new MongoDB.Bson.BsonRegularExpression(userName, "i")); // Case-insensitive search on 'name' field
            return await _user.Find(filter).ToListAsync();
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var filter = Builders<User>.Filter.Ne(u => u.fullname, null);
            return await _user.Find(filter).ToListAsync();
        }

        public async Task<Relationship> GetRelationshipAsync(string userId, string friendId)
        {
            var filter = Builders<Relationship>.Filter.Eq("userId", userId) & Builders<Relationship>.Filter.Eq("friendId", friendId);
            return await _relationship.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<FriendRequest> GetRequestAsync(string makerId, string receiverId)
        {
            var filter = Builders<FriendRequest>.Filter.Eq("makerId", makerId) &
                Builders<FriendRequest>.Filter.Eq("receiverId", receiverId);

            FriendRequest fr = await _friendRequests.Find(filter).FirstOrDefaultAsync();

            if (fr == null)
            {
                fr = new FriendRequest();
            }

            return fr;
        }

        public async Task<bool> CreateFriendRequest(string makerId, string receiverId)
        {
            var filter = Builders<FriendRequest>.Filter.Eq("makerId", makerId) &
                Builders<FriendRequest>.Filter.Eq("receiverId", receiverId);

            var filter1 = Builders<FriendRequest>.Filter.Eq("makerId", receiverId) &
                Builders<FriendRequest>.Filter.Eq("receiverId", makerId);

            var fr = _friendRequests.Find(filter).FirstOrDefault();
            var fr1 = _friendRequests.Find(filter1).FirstOrDefault();

            if (fr1 != null)
            {
                //have someone sent you a friendrequest!
                return false;
            }
            else if (fr != null)
            {
                return true;
            }
            else
            {
                var friendRequest = new FriendRequest
                {
                    makerId = makerId,
                    receiverId = receiverId
                };

                try
                {
                    await _friendRequests.InsertOneAsync(friendRequest);
                    return true; // Trả về true nếu không có ngoại lệ nào xảy ra
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex); // Ghi lại lỗi
                    return false; // Trả về false nếu có lỗi
                }
            }
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