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
        public UpdateUserController(IUpdateUserRepository repository)
        {
            _repository = repository;
        }

        //public UpdateUserController(ILogger logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {  
            return View(user);
        }

        [HttpPost]
        public IActionResult UpdateUserInfor(User updatedUser)
        {
            if (ModelState.IsValid)
            {
                var result = _repository.UpdateUser(updatedUser.id, updatedUser.fullname, updatedUser.gender, updatedUser.birthday, updatedUser.email);

                    return NoContent();
            }

            return BadRequest(ModelState);

        }
    }
}
