using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace ChatApp.Models
{
    [CollectionName("Roles")]
    public class ApplicationRole : MongoIdentityUser
    {
    }
}