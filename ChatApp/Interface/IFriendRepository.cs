using AddFriend.Models;
using ChatApp.Models;

namespace ChatApp.Interface
{
    public interface IFriendRepository
    {
        Task<User> GetUserAsync(string id);
        Task<Relationship> GetRelationshipAsync(string id);
        Task<bool> DeleteRelationship(Relationship relationship);
        Task<bool> CreateFriendRequest(string makeId, string receiverId);
        Task<bool> CancelRequestAsync(string makerId, string receiverId);
        Task<FriendRequest> GetRequestAsync(string makerId, string receiverId);
        Task<bool> AcceptRequestAsync(string makerId, string receiverId);

    }
}
