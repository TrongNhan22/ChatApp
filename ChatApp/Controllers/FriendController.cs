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

            var sendRequest = await _friend.GetRequestAsync("6585901a24c2c50f943af18e", userId);
            var receiveRequest = await _friend.GetRequestAsync(userId, "6585901a24c2c50f943af18e");

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
            var result = await _friend.CreateFriendRequest("6585901a24c2c50f943af18e", receiverId);

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
            var result = await _friend.CancelRequestAsync("6585901a24c2c50f943af18e", receiverId);

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
            var result = await _friend.AcceptRequestAsync("6585901a24c2c50f943af18e", makerId);

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
