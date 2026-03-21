using MechanicApp.Data;
using MechanicApp.Interfaces;
using MechanicApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MechanicApp.Repositories;

public class ConversationRepository : IConversationRepository
{
    private readonly DataContext _context;

    public ConversationRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Conversation>> GetConversationsAsync()
    {
        return await _context.Conversations
            .Include(c => c.Client)
                .ThenInclude(cl => cl.User)
            .Include(c => c.Mechanic)
                .ThenInclude(m => m.User)
            .OrderByDescending(c => c.LastMessageAt ?? c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Conversation?> GetConversationByIdAsync(int id)
    {
        return await _context.Conversations
            .Include(c => c.Client)
                .ThenInclude(cl => cl.User)
            .Include(c => c.Mechanic)
                .ThenInclude(m => m.User)
            .Include(c => c.ServiceRequest)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<ICollection<Conversation>> GetConversationsByClientAsync(int clientId)
    {
        return await _context.Conversations
            .Include(c => c.Mechanic)
                .ThenInclude(m => m.User)
            .Include(c => c.ServiceRequest)
            .Where(c => c.ClientId == clientId)
            .OrderByDescending(c => c.LastMessageAt ?? c.CreatedAt)
            .ToListAsync();
    }

    public async Task<ICollection<Conversation>> GetConversationsByMechanicAsync(int mechanicId)
    {
        return await _context.Conversations
            .Include(c => c.Client)
                .ThenInclude(cl => cl.User)
            .Include(c => c.ServiceRequest)
            .Where(c => c.MechanicId == mechanicId)
            .OrderByDescending(c => c.LastMessageAt ?? c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Conversation?> GetConversationWithMessagesAsync(int id)
    {
        return await _context.Conversations
            .Include(c => c.Client)
                .ThenInclude(cl => cl.User)
            .Include(c => c.Mechanic)
                .ThenInclude(m => m.User)
            .Include(c => c.ServiceRequest)
            .Include(c => c.Messages)
                .ThenInclude(m => m.Sender)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<ICollection<Conversation>> GetActiveConversationsAsync()
    {
        return await _context.Conversations
            .Include(c => c.Client)
                .ThenInclude(cl => cl.User)
            .Include(c => c.Mechanic)
                .ThenInclude(m => m.User)
            .Where(c => c.IsActive)
            .OrderByDescending(c => c.LastMessageAt ?? c.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> ConversationExistsAsync(int id)
    {
        return await _context.Conversations.AnyAsync(c => c.Id == id);
    }

    public async Task<bool> CreateConversationAsync(Conversation conversation)
    {
        _context.Conversations.Add(conversation);
        return await SaveAsync();
    }

    public async Task<bool> UpdateConversationAsync(Conversation conversation)
    {
        _context.Conversations.Update(conversation);
        return await SaveAsync();
    }

    public async Task<bool> DeleteConversationAsync(Conversation conversation)
    {
        _context.Conversations.Remove(conversation);
        return await SaveAsync();
    }

    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
