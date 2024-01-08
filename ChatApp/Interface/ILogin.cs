using ChatApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChatApp.Interface
{
    public interface ILogin
    {
        public Task<bool> GetAccountAsync(User user);
        public Task<User> GetUser(User user);

        public Task<bool> CreateUser(User user);

    }
}
