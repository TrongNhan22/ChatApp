using AddFriend.Models;
using ChatApp.Models;
using ChatApp.Repository;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ChatApp.Controllers
{
    public class SearchFriendController : Controller
    {
        private readonly ISearchFriendRepository _searchFriend;

        public SearchFriendController(ISearchFriendRepository searchFriend)
        {
            _searchFriend = searchFriend;          
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm)
        {
            if (Globals.user_login == null)
            {
                return Redirect("~/Login");
            }
            string currentId = Globals.user_login.id;
            List<User> friends;
            List<UserRelationshipModel> userrelationship = new List<UserRelationshipModel>();
            if (string.IsNullOrEmpty(searchTerm))
            {
                friends=await _searchFriend.GetUsersAsync();
            }
            else
            {
                friends = await _searchFriend.GetUserByNameAsync(searchTerm);
            }

            if(friends!=null)
            {
                foreach (var user in friends)
                {
                    if(user!=null && user.id != currentId && user.id != null)
                    {
                        Relationship relationship = new Relationship();
                        relationship = await _searchFriend.GetRelationshipAsync(currentId , user.id);
                        var sendRequest = await _searchFriend.GetRequestAsync(currentId, user.id);
                        var receiveRequest = await _searchFriend.GetRequestAsync(user.id, currentId);

                        FriendRequest friendRequest = new FriendRequest();
                        if (sendRequest.id != null && receiveRequest.id == null)
                        {
                            friendRequest = sendRequest;
                        }
                        if (sendRequest.id == null && receiveRequest.id != null)
                        {
                            friendRequest = receiveRequest;
                        }

                        userrelationship.Add(new UserRelationshipModel(user, relationship, friendRequest));
                    }
                }
            }
                
            return View(userrelationship);
        }

        [HttpPost]
        public async Task<IActionResult> MakeFriendRequest(string receiverId)
        {
            string currentId = Globals.user_login.id;
            var result = await _searchFriend.CreateFriendRequest(currentId, receiverId);

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
        public async Task<IActionResult> CancelRequest(string receiverId)
        {
            string currentId = Globals.user_login.id;
            var result = await _searchFriend.CancelRequestAsync(currentId, receiverId);

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
            var result = await _searchFriend.AcceptRequestAsync(currentId, makerId);

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
