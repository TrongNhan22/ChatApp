using ChatApp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    public class ChatController : Controller

    {
        public IActionResult Index()
        {
            //db call to get message history

            return View();
        }


    }
}
