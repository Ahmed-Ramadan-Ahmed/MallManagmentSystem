using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using MallManagmentSystem.Models;

namespace MallManagmentSystem.Interfaces
{
    public interface IStatisticsService
    {
        Task<FinancialStatistics> GetFinancialStatisticsAsync(int mallId, DateTime startDate, DateTime endDate);
        Task<StoreStatistics> GetStoreStatisticsAsync(int mallId);
        Task<EmployeeStatistics> GetEmployeeStatisticsAsync(int mallId);
        Task<RenterStatistics> GetRenterStatisticsAsync(int mallId);
        Task<LoanStatistics> GetLoanStatisticsAsync(int mallId);
        Task<DebitStatistics> GetDebitStatisticsAsync(int mallId);
        Task<ContractStatistics> GetContractStatisticsAsync(int mallId);
        Task<Dictionary<string, decimal>> GetRevenueDataAsync(int mallId, DateTime startDate, DateTime endDate);
        Task<Dictionary<string, decimal>> GetOccupancyDataAsync(int mallId);
        Task<Dictionary<string, int>> GetPaymentStatusDataAsync(int mallId);
        Task<Dictionary<string, decimal>> GetEmployeePerformanceDataAsync(int mallId);
    }
}