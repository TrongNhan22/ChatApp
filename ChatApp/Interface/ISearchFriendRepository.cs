using ChatApp.Models;

namespace ChatApp.Repository
{
    public interface ISearchFriendRepository
    {
        Task<List<User>> GetUserByNameAsync(string useName);
    }
}
