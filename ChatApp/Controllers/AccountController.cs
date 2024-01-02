using ChatApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    public class AccountController : Controller
    {
        //    private readonly UserManager<AppUser> _userManager;
        //    private readonly SignInManager<AppUser> _signInManager;

        //    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        //    {
        //        _signInManager = signInManager;
        //        _userManager = userManager;
        //    }

        public IActionResult Login()
        {
            var respone = new LoginViewModel();
            return View(respone);
        }

        //    [HttpPost]
        //    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        //    {
        //        if (!ModelState.IsValid) return View(loginViewModel);
        //        var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
        //        if (user != null)
        //        {
        //            var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
        //            if (passwordCheck)
        //            {
        //                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
        //                if (result.Succeeded)
        //                {
        //                    return RedirectToAction();

        //                }
        //            }
        //            TempData["Error"] = "Wrong credentials";
        //            return View(loginViewModel);
        //        }
        //        TempData["Error"] = "Wrong credentials";
        //        return View(loginViewModel);
        //    }

    }
}
