using AddFriend.Models;

namespace ChatApp.Models
{
    public class UserRelationshipModel
    {
        public User User { get; set; }
        public Relationship? Relationship { get; set; }

        public FriendRequest FriendRequest { get; set; }

        public UserRelationshipModel(User user, Relationship relationship, FriendRequest friendRequest)
        {
            User = user;
            Relationship = relationship;
            FriendRequest = friendRequest;
        }
    }
}
