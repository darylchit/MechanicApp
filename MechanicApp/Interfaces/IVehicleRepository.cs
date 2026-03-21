using MechanicApp.Models;

namespace MechanicApp.Interfaces;

public interface IVehicleRepository
{
    Task<ICollection<Vehicle>> GetVehiclesAsync();
    Task<Vehicle?> GetVehicleByIdAsync(int id);
    Task<ICollection<Vehicle>> GetVehiclesByClientAsync(int clientId);
    Task<Vehicle?> GetVehicleWithServiceRequestsAsync(int id);
    Task<bool> VehicleExistsAsync(int id);
    Task<bool> CreateVehicleAsync(Vehicle vehicle);
    Task<bool> UpdateVehicleAsync(Vehicle vehicle);
    Task<bool> DeleteVehicleAsync(Vehicle vehicle);
    Task<bool> SaveAsync();
}
