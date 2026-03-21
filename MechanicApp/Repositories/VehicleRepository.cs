using MechanicApp.Data;
using MechanicApp.Interfaces;
using MechanicApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MechanicApp.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly DataContext _context;

    public VehicleRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Vehicle>> GetVehiclesAsync()
    {
        return await _context.Vehicles
            .Include(v => v.Client)
                .ThenInclude(c => c.User)
            .OrderBy(v => v.Id)
            .ToListAsync();
    }

    public async Task<Vehicle?> GetVehicleByIdAsync(int id)
    {
        return await _context.Vehicles
            .Include(v => v.Client)
                .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<ICollection<Vehicle>> GetVehiclesByClientAsync(int clientId)
    {
        return await _context.Vehicles
            .Where(v => v.ClientId == clientId)
            .OrderBy(v => v.Year)
            .ThenBy(v => v.Make)
            .ToListAsync();
    }

    public async Task<Vehicle?> GetVehicleWithServiceRequestsAsync(int id)
    {
        return await _context.Vehicles
            .Include(v => v.Client)
                .ThenInclude(c => c.User)
            .Include(v => v.ServiceRequests)
                .ThenInclude(sr => sr.Mechanic)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<bool> VehicleExistsAsync(int id)
    {
        return await _context.Vehicles.AnyAsync(v => v.Id == id);
    }

    public async Task<bool> CreateVehicleAsync(Vehicle vehicle)
    {
        _context.Vehicles.Add(vehicle);
        return await SaveAsync();
    }

    public async Task<bool> UpdateVehicleAsync(Vehicle vehicle)
    {
        _context.Vehicles.Update(vehicle);
        return await SaveAsync();
    }

    public async Task<bool> DeleteVehicleAsync(Vehicle vehicle)
    {
        _context.Vehicles.Remove(vehicle);
        return await SaveAsync();
    }

    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
