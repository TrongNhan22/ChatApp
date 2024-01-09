using ChatApp.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace AddFriend.Models
{
    public class Relationship
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        [BsonElement("userId")]
        [JsonPropertyName("userId")]
        public string? userId { get; set; }
        [BsonIgnoreIfNull]
        public User? user { get; set; }
        [BsonElement("friendId")]
        [JsonPropertyName("friendId")]
        public string? friendId { get; set; }
        [BsonIgnoreIfNull]
        public User? friend { get; set; }
    }
}
