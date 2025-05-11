using MallManagmentSystem.Models;

namespace MallManagmentSystem.Interfaces
{
    public interface IRenterService
    {
        Task<List<Renter>> GetAllRentersAsync();
        Task<Renter> GetRenterByIdAsync(int id);
        Task<bool> CreateRenterAsync(Renter renter);
        Task<bool> UpdateRenterAsync(int id, Renter renter);
        Task<bool> DeleteRenterAsync(int id);
        Task<List<Store>> GetStoresByRenterIdAsync(int renterId);
        Task<bool> AddStoreToRenterAsync(int renterId, int storeId);
        Task<bool> RemoveStoreFromRenterAsync(int renterId, int storeId);
        Task<bool> AddRentContractAsync(int renterId, StoreRentContract contract);
        Task<bool> UpdateRentContractAsync(int renterId, int contractId, StoreRentContract contract);
        Task<List<StoreRentContract>> GetRentContractsByRenterIdAsync(int renterId);
        Task<bool> AddStorePenaltyAsync(int renterId, int storeId, StorePenalty penalty);
        Task<bool> RemoveStorePenaltyAsync(int renterId, int storeId, int penaltyId);
        Task<List<StorePenalty>> GetStorePenaltiesByRenterIdAsync(int renterId);
        Task<bool> AddRenterPaymentAsync(int renterId, RentInvoice Invoice);
        Task<List<RentInvoice>> GetRenterInvoicesByRenterIdAsync(int renterId, DateTime startDate, DateTime endDate);
    }
} 