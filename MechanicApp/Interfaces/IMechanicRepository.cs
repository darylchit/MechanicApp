using MechanicApp.Models;

namespace MechanicApp.Interfaces;

public interface IMechanicRepository
{
    Task<ICollection<Mechanic>> GetMechanicsAsync();
    Task<Mechanic?> GetMechanicByIdAsync(int id);
    Task<Mechanic?> GetMechanicByUserIdAsync(string userId);
    Task<ICollection<Mechanic>> GetAvailableMechanicsAsync();
    Task<ICollection<Mechanic>> GetMechanicsByShopAsync(int shopId);
    Task<Mechanic?> GetMechanicWithServiceRequestsAsync(int id);
    Task<bool> MechanicExistsAsync(int id);
    Task<bool> CreateMechanicAsync(Mechanic mechanic);
    Task<bool> UpdateMechanicAsync(Mechanic mechanic);
    Task<bool> DeleteMechanicAsync(Mechanic mechanic);
    Task<bool> SaveAsync();
}
