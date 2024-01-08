using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ChatApp.ViewModels
{
    public class MessageViewModel
    {
        public string SenderId { get; set; } = null!;
        public string? Media { get; set; }
        public string? Content { get; set; }
        public string? Date { get; set; }
    }
}
