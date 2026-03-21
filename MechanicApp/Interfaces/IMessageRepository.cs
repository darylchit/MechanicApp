using MechanicApp.Models;

namespace MechanicApp.Interfaces;

public interface IMessageRepository
{
    Task<ICollection<Message>> GetMessagesAsync();
    Task<Message?> GetMessageByIdAsync(int id);
    Task<ICollection<Message>> GetMessagesByConversationAsync(int conversationId);
    Task<ICollection<Message>> GetUnreadMessagesByUserAsync(string userId);
    Task<bool> MessageExistsAsync(int id);
    Task<bool> CreateMessageAsync(Message message);
    Task<bool> UpdateMessageAsync(Message message);
    Task<bool> DeleteMessageAsync(Message message);
    Task<bool> MarkAsReadAsync(int messageId);
    Task<bool> SaveAsync();
}
