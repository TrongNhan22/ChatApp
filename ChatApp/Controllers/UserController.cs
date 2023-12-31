﻿using ChatApp.Interface;
using ChatApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    public class UserController : Controller
    {
        //User user_login = Globals.user;
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
                User get_user = await _ilogin.GetUser(user);
                Globals.user_login = get_user;
                return Redirect("/");
            }
            // If the user does not exist, redirect back to the login page
            TempData["Error"] = "Email or password is not correct!";
            return Redirect("/Login");
        }

        public async Task<IActionResult> Signup(User user)
        {
            bool account = await _ilogin.CreateUser(user);
            TempData["Error"] = null;
            TempData["Noti"] = null;
            if (account)
            {
                // If the user exists, redirect to the home page
                TempData["Noti"] = "New account was created succcessfully!";
                return Redirect("/sign-up");
            }
            // If the user does not exist, redirect back to the login page
            TempData["Error"] = "Email is already used!";
            return Redirect("/sign-up");
        }
    }
}
