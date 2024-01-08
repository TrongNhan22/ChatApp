using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatApp.Models
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; }
        [BsonElement("sender_id")]
        public string SenderId { get; set; }
        [BsonElement("receiver_id")]
        public string ReceiverId { get; set; }
        //public DateTime Date { get; set; }
        [BsonElement("media")]
        public string? Media { get; set; }
        [BsonElement("content")]
        public string? Content { get; set; }
        [BsonElement("date")]
        public DateTime Date { get; set; }
    }
}