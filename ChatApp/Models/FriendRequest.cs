using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace ChatApp.Models
{
    public class FriendRequest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        [BsonElement("makerId")]
        [JsonPropertyName("makerId")]
        public string? makerId { get; set; }
        [BsonIgnoreIfNull]
        public User? maker { get; set; }
        [BsonElement("receiverId")]
        [JsonPropertyName("receiverId")]
        public string? receiverId { get; set; }
        [BsonIgnoreIfNull]
        public User? receiver { get; set; }
    }
}
