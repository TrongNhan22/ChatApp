using AddFriend.Models;
using ChatApp.Models;

namespace ChatApp.Repository
{
    public interface ISearchFriendRepository
    {
        Task<List<User>> GetUserByNameAsync(string useName);
        Task<List<User>> GetUsersAsync();
        Task<Relationship> GetRelationshipAsync(string userId, string friendId);
        Task<FriendRequest> GetRequestAsync(string makerId, string receiverId);
        Task<bool> CancelRequestAsync(string makerId, string receiverId);
        Task<bool> CreateFriendRequest(string makerId, string receiverId);
        Task<bool> AcceptRequestAsync(string makerId, string receiverId);

    }
}
