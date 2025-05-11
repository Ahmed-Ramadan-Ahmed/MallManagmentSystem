using MallManagmentSystem.Models;

namespace MallManagmentSystem.Interfaces
{
    public interface IPenaltyService
    {
        Task<List<StorePenalty>> GetAllStorePenaltiesAsync();
        Task<StorePenalty> GetStorePenaltyByIdAsync(int id);
        Task<bool> CreateStorePenaltyAsync(StorePenalty penalty);
        Task<bool> UpdateStorePenaltyAsync(int id, StorePenalty penalty);
        Task<bool> DeleteStorePenaltyAsync(int id);
        Task<List<StorePenalty>> GetPenaltiesByStoreIdAsync(int storeId);
        Task<List<StorePenalty>> GetPenaltiesByRenterIdAsync(int renterId);
        Task<List<StorePenalty>> GetPenaltiesByMallIdAsync(int mallId);
        Task<List<StorePenalty>> GetActivePenaltiesAsync();
        Task<List<StorePenalty>> GetPaidPenaltiesAsync();
        Task<List<StorePenalty>> GetUnpaidPenaltiesAsync();
        Task<bool> MarkPenaltyAsPaidAsync(int penaltyId);
        Task<PenaltySummary> GetPenaltySummaryByMallIdAsync(int mallId);
        Task<PenaltySummary> GetPenaltySummaryByStoreIdAsync(int storeId);
    }

    public class PenaltySummary
    {
        public int TotalPenalties { get; set; }
        public int ActivePenalties { get; set; }
        public int PaidPenalties { get; set; }
        public int UnpaidPenalties { get; set; }
        public decimal TotalPenaltyAmount { get; set; }
        public decimal PaidPenaltyAmount { get; set; }
        public decimal UnpaidPenaltyAmount { get; set; }
        public double PenaltyPaymentRate { get; set; }
    }
} 