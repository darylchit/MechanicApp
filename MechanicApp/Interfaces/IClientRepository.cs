using MechanicApp.Models;

namespace MechanicApp.Interfaces;

public interface IClientRepository
{
    Task<ICollection<Client>> GetClientsAsync();
    Task<Client?> GetClientByIdAsync(int id);
    Task<Client?> GetClientByUserIdAsync(string userId);
    Task<Client?> GetClientWithVehiclesAsync(int id);
    Task<Client?> GetClientWithServiceRequestsAsync(int id);
    Task<bool> ClientExistsAsync(int id);
    Task<bool> CreateClientAsync(Client client);
    Task<bool> UpdateClientAsync(Client client);
    Task<bool> DeleteClientAsync(Client client);
    Task<bool> SaveAsync();
}
