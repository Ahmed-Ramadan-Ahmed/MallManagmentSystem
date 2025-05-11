using MallManagmentSystem.Models;

namespace MallManagmentSystem.Interfaces
{
    public interface IContractService
    {
        Task<List<StoreRentContract>> GetAllRentContractsAsync();
        Task<StoreRentContract> GetRentContractByIdAsync(int id);
        Task<bool> CreateRentContractAsync(StoreRentContract contract);
        Task<bool> UpdateRentContractAsync(int id, StoreRentContract contract);
        Task<bool> DeleteRentContractAsync(int id);
        Task<List<StoreRentContract>> GetContractsByStoreIdAsync(int storeId);
        Task<List<StoreRentContract>> GetContractsByRenterIdAsync(int renterId);
        Task<List<StoreRentContract>> GetContractsByMallIdAsync(int mallId);
        Task<List<StoreRentContract>> GetActiveContractsAsync();
        Task<List<StoreRentContract>> GetExpiredContractsAsync();
        Task<List<StoreRentContract>> GetContractsExpiringSoonAsync(int daysThreshold);
        Task<bool> RenewContractAsync(int contractId, DateTime newEndDate);
        Task<bool> TerminateContractAsync(int contractId, string reason);
        Task<ContractSummary> GetContractSummaryByMallIdAsync(int mallId);
    }

    public class ContractSummary
    {
        public int TotalContracts { get; set; }
        public int ActiveContracts { get; set; }
        public int ExpiredContracts { get; set; }
        public int ContractsExpiringSoon { get; set; }
        public decimal TotalRentAmount { get; set; }
        public decimal ActiveRentAmount { get; set; }
        public double ContractRenewalRate { get; set; }
    }
} 