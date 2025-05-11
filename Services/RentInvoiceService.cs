using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MallManagmentSystem.Data;
using MallManagmentSystem.Interfaces;
using MallManagmentSystem.Models;

namespace MallManagmentSystem.Services
{
    public class RentInvoiceService : IRentInvoiceService
    {
        private readonly MallDBContext _context;

        public RentInvoiceService(MallDBContext context)
        {
            _context = context;
        }

        public async Task GenerateMonthlyInvoicesAsync()
        {
            try
            {
                var today = DateTime.UtcNow.Date;
                var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                // Get all active store contracts
                var activeContracts = await _context.StoreRentContracts
                    .Include(c => c.Store)
                    .Include(c => c.Renter)
                    .Where(c => c.Status == "Active" && c.StartDate <= lastDayOfMonth && c.EndDate >= firstDayOfMonth)
                    .ToListAsync();

                foreach (var contract in activeContracts)
                {
                    // Check if invoice already exists for this month
                    var existingInvoice = await _context.RentInvoices
                        .FirstOrDefaultAsync(i => i.StoreId == contract.StoreId && 
                                                i.InvoiceDate.Year == firstDayOfMonth.Year && 
                                                i.InvoiceDate.Month == firstDayOfMonth.Month);

                    if (existingInvoice == null)
                    {
                        await GenerateInvoiceForStoreAsync(contract.StoreId, firstDayOfMonth);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to generate monthly invoices", ex);
            }
        }

        public async Task<RentInvoice> GenerateInvoiceForStoreAsync(int storeId, DateTime invoiceDate)
        {
            try
            {
                var store = await _context.Stores
                    .Include(s => s.Contracts)
                    .Include(s => s.Renters)
                    .FirstOrDefaultAsync(s => s.Id == storeId);

                if (store == null)
                    throw new Exception($"Store with ID {storeId} not found");

                var activeContract = store.Contracts
                    .FirstOrDefault(c => c.Status == "Active" && 
                                       c.StartDate <= invoiceDate && 
                                       c.EndDate >= invoiceDate);

                if (activeContract == null)
                    throw new Exception($"No active contract found for store {storeId} on {invoiceDate}");

                var renter = store.Renters.FirstOrDefault();
                if (renter == null)
                    throw new Exception($"No renter found for store {storeId}");

                // Get renter's debit amount
                var debitAmount = await _context.DebitForRenters
                    .Where(d => d.RenterId == renter.Id && d.IsActive)
                    .SumAsync(d => d.Amount);

                var invoice = new RentInvoice
                {
                    StoreId = storeId,
                    RenterId = renter.Id,
                    InvoiceDate = invoiceDate,
                    DueDate = invoiceDate.AddDays(7), // Due in 7 days
                    Amount = activeContract.MonthlyRent,
                    DebitAmount = debitAmount,
                    IsPending = true
                };

                await _context.RentInvoices.AddAsync(invoice);
                await _context.SaveChangesAsync();

                return invoice;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to generate invoice for store {storeId}", ex);
            }
        }

        public async Task<RentInvoice> GetInvoiceByIdAsync(int invoiceId)
        {
            try
            {
                return await _context.RentInvoices
                    .Include(i => i.Store)
                    .Include(i => i.Renter)
                    .FirstOrDefaultAsync(i => i.Id == invoiceId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get invoice {invoiceId}", ex);
            }
        }

        public async Task<List<RentInvoice>> GetInvoicesByStoreAsync(int storeId)
        {
            try
            {
                return await _context.RentInvoices
                    .Include(i => i.Store)
                    .Include(i => i.Renter)
                    .Where(i => i.StoreId == storeId)
                    .OrderByDescending(i => i.InvoiceDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get invoices for store {storeId}", ex);
            }
        }

        public async Task<List<RentInvoice>> GetInvoicesByRenterAsync(int renterId)
        {
            try
            {
                return await _context.RentInvoices
                    .Include(i => i.Store)
                    .Include(i => i.Renter)
                    .Where(i => i.RenterId == renterId)
                    .OrderByDescending(i => i.InvoiceDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get invoices for renter {renterId}", ex);
            }
        }

        public async Task<List<RentInvoice>> GetOverdueInvoicesAsync()
        {
            try
            {
                var today = DateTime.UtcNow.Date;
                return await _context.RentInvoices
                    .Include(i => i.Store)
                    .Include(i => i.Renter)
                    .Where(i => i.Isoverdue == true)
                    .OrderBy(i => i.DueDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get overdue invoices", ex);
            }
        }

        public async Task<List<RentInvoice>> GetPendingInvoicesAsync()
        {
            try
            {
                return await _context.RentInvoices
                    .Include(i => i.Store)
                    .Include(i => i.Renter)
                    .Where(i => i.IsPending)
                    .OrderBy(i => i.DueDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get pending invoices", ex);
            }
        }
    }
} 