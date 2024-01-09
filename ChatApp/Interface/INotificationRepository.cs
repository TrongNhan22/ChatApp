using ChatApp.Models;

namespace ChatApp.Interface
{
    public interface INotificationRepository
    {
        Task<User> GetUserAsync(string id);
        Task<List<FriendRequest>> GetListRequestAsync(string receiverId);
        Task<bool> CancelRequestAsync(string makerId, string receiverId);
        Task<bool> AcceptRequestAsync(string makerId, string receiverId);
    }
}
