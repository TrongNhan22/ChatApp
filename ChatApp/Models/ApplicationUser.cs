using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Driver;
using MongoDbGenericRepository.Attributes;

namespace ChatApp.Models
{
    [CollectionName("Users")]
    public class ApplicationUser:MongoIdentityUser
    {
    }
}
