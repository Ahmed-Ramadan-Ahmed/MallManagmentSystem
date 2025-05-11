using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using MallManagmentSystem.Models;

namespace MallManagmentSystem.Interfaces
{
    public interface IPrintService
    {
        // Invoice Printing
        Task<byte[]> PrintRentInvoiceAsync(int invoiceId);
        Task<byte[]> PrintRentInvoiceWithPaymentsAsync(int invoiceId);
        Task<byte[]> PrintMultipleInvoicesAsync(List<int> invoiceIds);

        // Contract Printing
        Task<byte[]> PrintStoreContractAsync(int contractId);
        Task<byte[]> PrintEmployeeContractAsync(int contractId);
        Task<byte[]> PrintMultipleContractsAsync(List<int> contractIds);

        // Statistics Reports
        Task<byte[]> PrintFinancialStatisticsAsync(int mallId, DateTime startDate, DateTime endDate);
        Task<byte[]> PrintStoreStatisticsAsync(int mallId);
        Task<byte[]> PrintEmployeeStatisticsAsync(int mallId);
        Task<byte[]> PrintRenterStatisticsAsync(int mallId);
        Task<byte[]> PrintContractStatisticsAsync(int mallId);

        // Charts
        Task<byte[]> GenerateRevenueChartAsync(int mallId, DateTime startDate, DateTime endDate);
        Task<byte[]> GenerateOccupancyChartAsync(int mallId);
        Task<byte[]> GeneratePaymentStatusChartAsync(int mallId);
        Task<byte[]> GenerateEmployeePerformanceChartAsync(int mallId);
    }
} 