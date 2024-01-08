using ChatApp.Models;

namespace ChatApp.ViewModels
{
    public class MessageViewModel
    {
        public string SenderId { get; set; }
        public string? ReceiverId { get; set; }
        public IEnumerable<Message>? Messages { get; set; }
        public string? Content { get; set; }
        public string? Media { get; set; }
        public string? Date { get; set; }
    }
}
