using ChatApp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    public class SearchFriendController : Controller
    {
        private readonly ISearchFriendRepository searchFriend;

        SearchFriendController(ISearchFriendRepository searchFriend)
        {
            this.searchFriend = searchFriend;
        }

        public IActionResult Index() { return View(); }

        //[HttpGet("search")]
        //public async Task<IActionResult> SearchFriend(string searchTerm) {
        //    var friends = await searchFriend.GetUserByNameAsync(searchTerm);
        //    return View(friends);
        //}
    }
}
