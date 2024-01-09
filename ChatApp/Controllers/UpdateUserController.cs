using Microsoft.AspNetCore.Mvc;
using ChatApp.Models;
using System.Globalization;
using ChatApp.Interface;
using Microsoft.Build.Logging;

namespace ChatApp.Controllers
{
    public class UpdateUserController : Controller
    {
        private readonly IUpdateUserRepository _repository;
        private readonly User user = Globals.user_login;
        private readonly ILogger _logger;
        public UpdateUserController(IUpdateUserRepository repository)
        {
            _repository = repository;
        }

        //public UpdateUserController(ILogger logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult UpdateUser()
        {
            return View(User);
        }

        [HttpPost]
        public IActionResult UpdateUserInfor(User u)
        { 
            _repository.UpdateUser(u.id, u.fullname, u.gender, u.birthday, u.email);

            return NoContent();
        }
    }
}
