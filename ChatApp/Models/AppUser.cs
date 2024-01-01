using Microsoft.AspNetCore.Identity;

namespace ChatApp.Models
{
    public class AppUser : IdentityUser
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Avatar { get; set; }
        public string? Birthday { get; set; }
        public string? Fullname { get; set; }
        public int? Gender { get; set; }

    }

}
