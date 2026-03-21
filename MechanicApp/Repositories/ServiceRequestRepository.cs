using MechanicApp.Data;
using MechanicApp.Interfaces;
using MechanicApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MechanicApp.Repositories;

public class ServiceRequestRepository : IServiceRequestRepository
{
    private readonly DataContext _context;

    public ServiceRequestRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<ICollection<ServiceRequest>> GetServiceRequestsAsync()
    {
        return await _context.ServiceRequests
            .Include(sr => sr.Client)
                .ThenInclude(c => c.User)
            .Include(sr => sr.Vehicle)
            .Include(sr => sr.Mechanic)
                .ThenInclude(m => m.User)
            .OrderByDescending(sr => sr.CreatedAt)
            .ToListAsync();
    }

    public async Task<ServiceRequest?> GetServiceRequestByIdAsync(int id)
    {
        return await _context.ServiceRequests
            .Include(sr => sr.Client)
                .ThenInclude(c => c.User)
            .Include(sr => sr.Vehicle)
            .Include(sr => sr.Mechanic)
                .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(sr => sr.Id == id);
    }

    public async Task<ICollection<ServiceRequest>> GetServiceRequestsByClientAsync(int clientId)
    {
        return await _context.ServiceRequests
            .Include(sr => sr.Vehicle)
            .Include(sr => sr.Mechanic)
                .ThenInclude(m => m.User)
            .Where(sr => sr.ClientId == clientId)
            .OrderByDescending(sr => sr.CreatedAt)
            .ToListAsync();
    }

    public async Task<ICollection<ServiceRequest>> GetServiceRequestsByMechanicAsync(int mechanicId)
    {
        return await _context.ServiceRequests
            .Include(sr => sr.Client)
                .ThenInclude(c => c.User)
            .Include(sr => sr.Vehicle)
            .Where(sr => sr.MechanicId == mechanicId)
            .OrderByDescending(sr => sr.CreatedAt)
            .ToListAsync();
    }

    public async Task<ICollection<ServiceRequest>> GetServiceRequestsByVehicleAsync(int vehicleId)
    {
        return await _context.ServiceRequests
            .Include(sr => sr.Mechanic)
                .ThenInclude(m => m.User)
            .Where(sr => sr.VehicleId == vehicleId)
            .OrderByDescending(sr => sr.CreatedAt)
            .ToListAsync();
    }

    public async Task<ICollection<ServiceRequest>> GetServiceRequestsByStatusAsync(ServiceStatus status)
    {
        return await _context.ServiceRequests
            .Include(sr => sr.Client)
                .ThenInclude(c => c.User)
            .Include(sr => sr.Vehicle)
            .Include(sr => sr.Mechanic)
                .ThenInclude(m => m.User)
            .Where(sr => sr.Status == status)
            .OrderByDescending(sr => sr.CreatedAt)
            .ToListAsync();
    }

    public async Task<ServiceRequest?> GetServiceRequestWithDetailsAsync(int id)
    {
        return await _context.ServiceRequests
            .Include(sr => sr.Client)
                .ThenInclude(c => c.User)
            .Include(sr => sr.Vehicle)
            .Include(sr => sr.Mechanic)
                .ThenInclude(m => m.User)
            .Include(sr => sr.Conversation)
                .ThenInclude(c => c.Messages)
            .FirstOrDefaultAsync(sr => sr.Id == id);
    }

    public async Task<bool> ServiceRequestExistsAsync(int id)
    {
        return await _context.ServiceRequests.AnyAsync(sr => sr.Id == id);
    }

    public async Task<bool> CreateServiceRequestAsync(ServiceRequest serviceRequest)
    {
        _context.ServiceRequests.Add(serviceRequest);
        return await SaveAsync();
    }

    public async Task<bool> UpdateServiceRequestAsync(ServiceRequest serviceRequest)
    {
        _context.ServiceRequests.Update(serviceRequest);
        return await SaveAsync();
    }

    public async Task<bool> DeleteServiceRequestAsync(ServiceRequest serviceRequest)
    {
        _context.ServiceRequests.Remove(serviceRequest);
        return await SaveAsync();
    }

    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
