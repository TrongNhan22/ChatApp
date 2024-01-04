using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ChatApp.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? avatar { get; set; }
        public string? birthday { get; set; }
        public string? fullname { get; set; }
        public string? gender { get; set; }
    }
}
