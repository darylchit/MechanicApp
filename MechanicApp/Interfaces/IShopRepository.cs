using MechanicApp.Models;

namespace MechanicApp.Interfaces;

public interface IShopRepository
{
    Task<ICollection<Shop>> GetShopsAsync();
    Task<Shop?> GetShopByIdAsync(int id);
    Task<Shop?> GetShopWithMechanicsAsync(int id);
    Task<bool> ShopExistsAsync(int id);
    Task<bool> CreateShopAsync(Shop shop);
    Task<bool> UpdateShopAsync(Shop shop);
    Task<bool> DeleteShopAsync(Shop shop);
    Task<bool> SaveAsync();
}
