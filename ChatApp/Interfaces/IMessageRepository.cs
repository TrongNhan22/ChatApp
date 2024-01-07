using ChatApp.Models;

namespace ChatApp.Interfaces
{
    public interface IMessageRepository
    {
        public Task<IEnumerable<Message>> GetListAsync();
        public Task<IEnumerable<Message>> GetListBySenderAndReceiverIdAsync(string senderId, string receiverId);
        public Task<Message> GetMessageByIdAsync(string id);
        public Task CreateAsync(Message newMessage);
        public Task UpdateAsync(string id, Message updatedMessage);
        public Task DeleteAsync(string id);
    }
}
