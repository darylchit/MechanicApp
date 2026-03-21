using MechanicApp.Data;
using MechanicApp.Interfaces;
using MechanicApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MechanicApp.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly DataContext _context;

    public ClientRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Client>> GetClientsAsync()
    {
        return await _context.Clients
            .Include(c => c.User)
            .OrderBy(c => c.Id)
            .ToListAsync();
    }

    public async Task<Client?> GetClientByIdAsync(int id)
    {
        return await _context.Clients
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Client?> GetClientByUserIdAsync(string userId)
    {
        return await _context.Clients
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<Client?> GetClientWithVehiclesAsync(int id)
    {
        return await _context.Clients
            .Include(c => c.User)
            .Include(c => c.Vehicles)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Client?> GetClientWithServiceRequestsAsync(int id)
    {
        return await _context.Clients
            .Include(c => c.User)
            .Include(c => c.ServiceRequests)
                .ThenInclude(sr => sr.Vehicle)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> ClientExistsAsync(int id)
    {
        return await _context.Clients.AnyAsync(c => c.Id == id);
    }

    public async Task<bool> CreateClientAsync(Client client)
    {
        _context.Clients.Add(client);
        return await SaveAsync();
    }

    public async Task<bool> UpdateClientAsync(Client client)
    {
        _context.Clients.Update(client);
        return await SaveAsync();
    }

    public async Task<bool> DeleteClientAsync(Client client)
    {
        _context.Clients.Remove(client);
        return await SaveAsync();
    }

    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
