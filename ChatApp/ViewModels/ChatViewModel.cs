using ChatApp.Models;

namespace ChatApp.ViewModels
{
    public class ChatViewModel
    {
        public string SenderId { get; set; } = null!;
        public string ReceiverId { get; set; } = null!;
        public IEnumerable<Message>? Messages { get; set; }
        public string? Content { get; set; }
        public IFormFile? Media { get; set; }
        public string? Date { get; set; }
        public string CurrentChatParterId { get; set; }
        public IEnumerable<string>? ChatUser { get; set; }
        public IEnumerable<ChatPartnerViewModel>? ChatPartners { get; set; }
    }
}
