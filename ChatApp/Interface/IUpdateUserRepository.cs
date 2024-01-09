using ChatApp.Models;

namespace ChatApp.Interface
{
    public interface IUpdateUserRepository
    {
        Task<User> GetUserById(string id);
        Task<bool> UpdateUser(string id, string fullname, string gender, string birhtday, string email);
    }
}
