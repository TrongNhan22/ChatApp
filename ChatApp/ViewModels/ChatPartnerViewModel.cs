using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ChatApp.ViewModels
{
    public class ChatPartnerViewModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; } = null!;
        public string? avatar { get; set; }
        public string? fullname { get; set; }
        public DateTime? lastInteractionTime { get; set; }
        public string? latestMessage { get; set;}
        public int? UnseenMessageCount { get; set; }    
    }
}
