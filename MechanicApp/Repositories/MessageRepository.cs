using MechanicApp.Data;
using MechanicApp.Interfaces;
using MechanicApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MechanicApp.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly DataContext _context;

    public MessageRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Message>> GetMessagesAsync()
    {
        return await _context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Conversation)
            .OrderByDescending(m => m.SentAt)
            .ToListAsync();
    }

    public async Task<Message?> GetMessageByIdAsync(int id)
    {
        return await _context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Conversation)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<ICollection<Message>> GetMessagesByConversationAsync(int conversationId)
    {
        return await _context.Messages
            .Include(m => m.Sender)
            .Where(m => m.ConversationId == conversationId)
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }

    public async Task<ICollection<Message>> GetUnreadMessagesByUserAsync(string userId)
    {
        return await _context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Conversation)
            .Where(m => m.Status != MessageStatus.Read && m.SenderId != userId)
            .OrderByDescending(m => m.SentAt)
            .ToListAsync();
    }

    public async Task<bool> MessageExistsAsync(int id)
    {
        return await _context.Messages.AnyAsync(m => m.Id == id);
    }

    public async Task<bool> CreateMessageAsync(Message message)
    {
        _context.Messages.Add(message);
        return await SaveAsync();
    }

    public async Task<bool> UpdateMessageAsync(Message message)
    {
        _context.Messages.Update(message);
        return await SaveAsync();
    }

    public async Task<bool> DeleteMessageAsync(Message message)
    {
        _context.Messages.Remove(message);
        return await SaveAsync();
    }

    public async Task<bool> MarkAsReadAsync(int messageId)
    {
        var message = await _context.Messages.FindAsync(messageId);
        if (message == null) return false;

        message.Status = MessageStatus.Read;
        message.ReadAt = DateTime.UtcNow;
        
        return await SaveAsync();
    }

    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
