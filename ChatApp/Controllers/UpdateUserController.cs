using Microsoft.AspNetCore.Mvc;
using ChatApp.Models;
using System.Globalization;
using ChatApp.Interface;
using Microsoft.Build.Logging;
using AddFriend.Models;

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

        
        public async Task<IActionResult> Index()
        {
            User u = new User();
            if (user.id != null) {
                u = await _repository.GetUserById(user.id);
            }
            
            return View(u);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserInfor(string id, string fullname, string gender, string birthday, string email)
        {
            var result = await _repository.UpdateUser(id, fullname, gender, birthday, email);

            if (result)
            {
                // Trả về một phản hồi thành công
                return Json(new { success = true });
            }
            else
            {
                // Trả về một lỗi nếu không tìm thấy mối quan hệ
                return Json(new { success = false, message = "Không thể cập nhật dữ liệu." });

            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            return Redirect("/Login");
            Globals.user_login = null;

        }
    }
}
