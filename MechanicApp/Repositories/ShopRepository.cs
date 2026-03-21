using MechanicApp.Data;
using MechanicApp.Interfaces;
using MechanicApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MechanicApp.Repositories;

public class ShopRepository : IShopRepository
{
    private readonly DataContext _context;

    public ShopRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Shop>> GetShopsAsync()
    {
        return await _context.Shops
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<Shop?> GetShopByIdAsync(int id)
    {
        return await _context.Shops
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Shop?> GetShopWithMechanicsAsync(int id)
    {
        return await _context.Shops
            .Include(s => s.Mechanics)
                .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<bool> ShopExistsAsync(int id)
    {
        return await _context.Shops.AnyAsync(s => s.Id == id);
    }

    public async Task<bool> CreateShopAsync(Shop shop)
    {
        _context.Shops.Add(shop);
        return await SaveAsync();
    }

    public async Task<bool> UpdateShopAsync(Shop shop)
    {
        _context.Shops.Update(shop);
        return await SaveAsync();
    }

    public async Task<bool> DeleteShopAsync(Shop shop)
    {
        _context.Shops.Remove(shop);
        return await SaveAsync();
    }

    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
