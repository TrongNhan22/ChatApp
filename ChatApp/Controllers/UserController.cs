using ChatApp.Interface;
using ChatApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    public class UserController : Controller
    {
        public User user;
        private readonly ILogin _ilogin;

        public UserController(ILogin ilogin)
        {
            _ilogin = ilogin;
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            bool account = await _ilogin.GetAccountAsync(user);
            TempData["Error"] = null;
            if (account)
            {
                // If the user exists, redirect to the home page
                user = await _ilogin.GetUser(user);
                return Redirect("/");
            }
            // If the user does not exist, redirect back to the login page
            TempData["Error"] = "Email or password is not correct!";
            //user.isLogin = 0;
            return Redirect("/Login");
        }
    }
}
