using AddFriend.Models;
using ChatApp.Data;
using ChatApp.Interface;
using ChatApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChatApp.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationRepository _notification;

        public NotificationController(INotificationRepository notification)
        {
            _notification = notification;
        }

        public async Task<IActionResult> Index()
        {
            List<FriendRequest> friendRequests;
            friendRequests = await _notification.GetListRequestAsync("6585901a24c2c50f943af18e");
            List<UserRelationshipModel> userRelationship = new List<UserRelationshipModel>();

            if(friendRequests.Count > 0)
            {
                foreach (FriendRequest friendRequest in friendRequests)
                {
                    User user = await _notification.GetUserAsync(friendRequest.makerId);
                    Relationship relationship = new Relationship();
                    userRelationship.Add(new UserRelationshipModel(user, relationship, friendRequest));
                }
            }

            return View(userRelationship);
        }

        [HttpPost]
        public async Task<IActionResult> CancelRequest(string receiverId)
        {
            var result = await _notification.CancelRequestAsync("6585901a24c2c50f943af18e", receiverId);

            if (result)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Không thể từ chối!" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AcceptRequest(string makerId)
        {
            var result = await _notification.AcceptRequestAsync("6585901a24c2c50f943af18e", makerId);

            if (result)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Không thể thực hiện thêm bạn bè!" });
            }
        }
    }
}
