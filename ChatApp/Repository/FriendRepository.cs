using AddFriend.Models;
using ChatApp.Data;
using ChatApp.Interface;
using ChatApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChatApp.Repository
{
    public class FriendRepository : IFriendRepository
    {
        private readonly IOptions<MongoDBSetting> _mongo;
        private readonly IMongoCollection<User> _user;
        private readonly IMongoCollection<Relationship> _relationship;
        private readonly IMongoCollection<FriendRequest> _friendRequests;
        

        public FriendRepository(IOptions<MongoDBSetting> mongo)
        {
            _mongo = mongo;
            MongoClient client = new MongoClient(_mongo.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(_mongo.Value.DatabaseName);
            _user = database.GetCollection<User>(_mongo.Value.userCollectionName);
            _relationship = database.GetCollection<Relationship>(_mongo.Value.relationshipCollectionName);
            _friendRequests = database.GetCollection<FriendRequest>(_mongo.Value.friendRequestCollectionName);
        }

        public async Task<User> GetUserAsync(string id)
        {
            var filter = Builders<User>.Filter.Eq("id", id);
            User user = new User();
            user = await _user.Find(filter).FirstOrDefaultAsync();
            return user;
        }

        public async Task<Relationship> GetRelationshipAsync(string id)
        {
            var filter = Builders<Relationship>.Filter.Eq("id", id);
            return await _relationship.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteRelationship(Relationship relationship)
        {
            var filter1 = Builders<Relationship>.Filter.Eq(rl => rl.id,  relationship.id);
            var filter2 = Builders<Relationship>.Filter.Eq(rl => rl.userId, relationship.friendId) &
                Builders<Relationship>.Filter.Eq(rl => rl.friendId, relationship.userId);
            //Because the relationship is 2 way direction so we need to delete 2 of them
            
            var deleteResult1 = await _relationship.DeleteOneAsync(filter1);
            var deleteResult2 = await _relationship.DeleteOneAsync(filter2);

            var result = deleteResult1.IsAcknowledged && deleteResult1.DeletedCount > 0 && deleteResult2.IsAcknowledged && deleteResult2.DeletedCount > 0;

            return result;
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
            else if(fr != null)
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

        public async Task<FriendRequest> GetRequestAsync(string makerId, string receiverId)
        {
            var filter = Builders<FriendRequest>.Filter.Eq("makerId", makerId) &
                Builders<FriendRequest>.Filter.Eq("receiverId", receiverId);

            FriendRequest fr = await _friendRequests.Find(filter).FirstOrDefaultAsync();

            if(fr == null )
            {
                fr = new FriendRequest();
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

            if(rl1 != null && rl2 != null)
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
