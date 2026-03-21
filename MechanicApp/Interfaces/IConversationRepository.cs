using MechanicApp.Models;

namespace MechanicApp.Interfaces;

public interface IConversationRepository
{
    Task<ICollection<Conversation>> GetConversationsAsync();
    Task<Conversation?> GetConversationByIdAsync(int id);
    Task<ICollection<Conversation>> GetConversationsByClientAsync(int clientId);
    Task<ICollection<Conversation>> GetConversationsByMechanicAsync(int mechanicId);
    Task<Conversation?> GetConversationWithMessagesAsync(int id);
    Task<ICollection<Conversation>> GetActiveConversationsAsync();
    Task<bool> ConversationExistsAsync(int id);
    Task<bool> CreateConversationAsync(Conversation conversation);
    Task<bool> UpdateConversationAsync(Conversation conversation);
    Task<bool> DeleteConversationAsync(Conversation conversation);
    Task<bool> SaveAsync();
}
