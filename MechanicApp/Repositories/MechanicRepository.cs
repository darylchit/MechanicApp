using MechanicApp.Data;
using MechanicApp.Interfaces;
using MechanicApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MechanicApp.Repositories;

public class MechanicRepository : IMechanicRepository
{
    private readonly DataContext _context;

    public MechanicRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Mechanic>> GetMechanicsAsync()
    {
        return await _context.Mechanics
            .Include(m => m.User)
            .Include(m => m.Shop)
            .OrderBy(m => m.Id)
            .ToListAsync();
    }

    public async Task<Mechanic?> GetMechanicByIdAsync(int id)
    {
        return await _context.Mechanics
            .Include(m => m.User)
            .Include(m => m.Shop)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Mechanic?> GetMechanicByUserIdAsync(string userId)
    {
        return await _context.Mechanics
            .Include(m => m.User)
            .Include(m => m.Shop)
            .FirstOrDefaultAsync(m => m.UserId == userId);
    }

    public async Task<ICollection<Mechanic>> GetAvailableMechanicsAsync()
    {
        return await _context.Mechanics
            .Include(m => m.User)
            .Include(m => m.Shop)
            .Where(m => m.IsAvailable)
            .ToListAsync();
    }

    public async Task<ICollection<Mechanic>> GetMechanicsByShopAsync(int shopId)
    {
        return await _context.Mechanics
            .Include(m => m.User)
            .Where(m => m.ShopId == shopId)
            .ToListAsync();
    }

    public async Task<Mechanic?> GetMechanicWithServiceRequestsAsync(int id)
    {
        return await _context.Mechanics
            .Include(m => m.User)
            .Include(m => m.Shop)
            .Include(m => m.ServiceRequests)
                .ThenInclude(sr => sr.Vehicle)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<bool> MechanicExistsAsync(int id)
    {
        return await _context.Mechanics.AnyAsync(m => m.Id == id);
    }

    public async Task<bool> CreateMechanicAsync(Mechanic mechanic)
    {
        _context.Mechanics.Add(mechanic);
        return await SaveAsync();
    }

    public async Task<bool> UpdateMechanicAsync(Mechanic mechanic)
    {
        _context.Mechanics.Update(mechanic);
        return await SaveAsync();
    }

    public async Task<bool> DeleteMechanicAsync(Mechanic mechanic)
    {
        _context.Mechanics.Remove(mechanic);
        return await SaveAsync();
    }

    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
