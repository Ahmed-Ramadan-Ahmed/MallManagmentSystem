using MallManagmentSystem.Models;

namespace MallManagmentSystem.Interfaces
{
    public interface IStoreService
    {
        Task<List<Store>> GetAllStoresAsync();
        Task<Store> GetStoreByIdAsync(int id);
        Task<bool> CreateStoreAsync(Store store);
        Task<bool> UpdateStoreAsync(int id, Store store);
        Task<bool> DeleteStoreAsync(int id);
        Task<List<Store>> GetStoresByMallIdAsync(int mallId);
        Task<List<Store>> GetStoresByRenterIdAsync(int renterId);
        Task<bool> AssignRenterToStoreAsync(int storeId, int renterId);
        Task<bool> RemoveRenterFromStoreAsync(int storeId);
        Task<bool> AddPenaltyToStoreAsync(int storeId, StorePenalty penalty);
        Task<bool> RemovePenaltyFromStoreAsync(int storeId, int penaltyId);
        Task<List<StorePenalty>> GetPenaltiesByStoreIdAsync(int storeId);
        Task<bool> UpdatePenaltyAsync(int storeId, int penaltyId, StorePenalty penalty);
        Task<bool> AddStoreToMallAsync(int storeId, int mallId);
    }
}
