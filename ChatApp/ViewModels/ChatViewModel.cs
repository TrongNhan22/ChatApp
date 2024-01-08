using ChatApp.Models;

namespace ChatApp.ViewModels
{
    public class ChatViewModel
    {
        public string SenderId { get; set; }
        public string? ReceiverId { get; set; }
        public IEnumerable<Message>? Messages { get; set; }
        public string? Content { get; set; }
        public string? Media { get; set; }
        public string? Date { get; set; }
        public IEnumerable<string>? ChatUser { get; set; }
        public IEnumerable<ChatPartnerViewModel>? ChatPartners { get; set; }
    }
}
