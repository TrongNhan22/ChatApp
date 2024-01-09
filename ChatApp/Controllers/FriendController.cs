using AddFriend.Models;
using ChatApp.Interface;
using ChatApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;

namespace ChatApp.Controllers
{
    public class FriendController : Controller
    {
        private readonly IFriendRepository _friend;

        public FriendController(IFriendRepository friend)
        {
            _friend = friend;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string userId, string relationshipId)
        {
            if (Globals.user_login == null)
            {
                return Redirect("~/Login");
            }
            string currentId = Globals.user_login.id;
            User friend = new User();
            friend = await _friend.GetUserAsync(userId);
            Relationship relationship;
            if (relationshipId != null && relationshipId != "null")
            {
                relationship = await _friend.GetRelationshipAsync(relationshipId);
            }
            else
            {
                relationship=new Relationship();
            }

            var sendRequest = await _friend.GetRequestAsync(currentId, userId);
            var receiveRequest = await _friend.GetRequestAsync(userId, currentId);

            FriendRequest friendRequest = new FriendRequest();
            if (sendRequest.id != null && receiveRequest.id == null)
            {
                friendRequest = sendRequest;
            }
            if(sendRequest.id == null && receiveRequest.id != null)
            {
                friendRequest = receiveRequest;
            }

            UserRelationshipModel userRelationship;

            //Truyen model
            userRelationship = new UserRelationshipModel(friend, relationship, friendRequest);
            

            return View(userRelationship);
        }

        [HttpPost]
        public async Task<IActionResult> Unfriend(string relationshipId)
        {

            // Tìm kiếm mối quan hệ bạn bè giữa người dùng hiện tại và người dùng có ID được truyền vào
            var relationship = await _friend.GetRelationshipAsync(relationshipId);

            if (relationship != null)
            {
                // Xóa mối quan hệ khỏi cơ sở dữ liệu
                bool result = await _friend.DeleteRelationship(relationship);

                // Trả về một phản hồi thành công
                return Json(new { success = true });
            }
            else
            {
                // Trả về một lỗi nếu không tìm thấy mối quan hệ
                return Json(new { success = false, message = "Không tìm thấy mối quan hệ bạn bè." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> MakeFriendRequest(string receiverId)
        {
            string currentId = Globals.user_login.id;
            var result = await _friend.CreateFriendRequest(currentId, receiverId);

            if (result)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Không thể thực hiện thêm bạn bè"});
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelRequest(string receiverId)
        {
            string currentId = Globals.user_login.id;
            var result = await _friend.CancelRequestAsync(currentId, receiverId);

            if (result)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Không thể thực hiện thêm bạn bè" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AcceptRequest(string makerId)
        {
            string currentId = Globals.user_login.id;
            var result = await _friend.AcceptRequestAsync(currentId, makerId);

            if (result)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Không thể thực hiện thêm bạn bè" });
            }
        }
    }
}
