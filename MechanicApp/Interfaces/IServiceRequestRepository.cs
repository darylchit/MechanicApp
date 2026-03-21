using MechanicApp.Models;

namespace MechanicApp.Interfaces;

public interface IServiceRequestRepository
{
    Task<ICollection<ServiceRequest>> GetServiceRequestsAsync();
    Task<ServiceRequest?> GetServiceRequestByIdAsync(int id);
    Task<ICollection<ServiceRequest>> GetServiceRequestsByClientAsync(int clientId);
    Task<ICollection<ServiceRequest>> GetServiceRequestsByMechanicAsync(int mechanicId);
    Task<ICollection<ServiceRequest>> GetServiceRequestsByVehicleAsync(int vehicleId);
    Task<ICollection<ServiceRequest>> GetServiceRequestsByStatusAsync(ServiceStatus status);
    Task<ServiceRequest?> GetServiceRequestWithDetailsAsync(int id);
    Task<bool> ServiceRequestExistsAsync(int id);
    Task<bool> CreateServiceRequestAsync(ServiceRequest serviceRequest);
    Task<bool> UpdateServiceRequestAsync(ServiceRequest serviceRequest);
    Task<bool> DeleteServiceRequestAsync(ServiceRequest serviceRequest);
    Task<bool> SaveAsync();
}
