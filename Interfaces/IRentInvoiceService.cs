using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using MallManagmentSystem.Models;

namespace MallManagmentSystem.Interfaces
{
    public interface IRentInvoiceService
    {
        // Invoice Generation
        Task GenerateMonthlyInvoicesAsync();
        Task<RentInvoice> GenerateInvoiceForStoreAsync(int storeId, DateTime invoiceDate);

        // Invoice Management
        Task<RentInvoice> GetInvoiceByIdAsync(int invoiceId);
        Task<List<RentInvoice>> GetInvoicesByStoreAsync(int storeId);
        Task<List<RentInvoice>> GetInvoicesByRenterAsync(int renterId);
        Task<List<RentInvoice>> GetOverdueInvoicesAsync();
        Task<List<RentInvoice>> GetPendingInvoicesAsync();
    }
} 