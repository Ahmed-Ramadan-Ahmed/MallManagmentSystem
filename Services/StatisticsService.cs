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
    public class StatisticsService : IStatisticsService
    {
        private readonly MallDBContext _context;

        public StatisticsService(MallDBContext context)
        {
            _context = context;
        }

        public async Task<FinancialStatistics> GetFinancialStatisticsAsync(int mallId, DateTime startDate, DateTime endDate)
        {
            var statistics = new FinancialStatistics
            {
                TotalRevenue = await _context.RentInvoices
                    .Where(i => i.Store.MallId == mallId && i.InvoiceDate >= startDate && i.InvoiceDate <= endDate)
                    .SumAsync(i => i.Amount),

                TotalExpenses = await _context.EmployeeLoans
                    .Where(l => l.Employee.MallId == mallId && l.LoanDate >= startDate && l.LoanDate <= endDate)
                    .SumAsync(l => l.Amount),

                TotalRentIncome = await _context.RentInvoices
                    .Where(i => i.Store.MallId == mallId && i.InvoiceDate >= startDate && i.InvoiceDate <= endDate)
                    .SumAsync(i => i.Amount),

                TotalLoans = await _context.EmployeeLoans
                    .Where(l => l.Employee.MallId == mallId && l.LoanDate >= startDate && l.LoanDate <= endDate)
                    .SumAsync(l => l.Amount),

                TotalDebits = await _context.DebitForRenters
                    .Where(d => d.Store.MallId == mallId && d.DebitDate >= startDate && d.IsActive)
                    .SumAsync(d => d.Amount)
            };

            statistics.NetProfit = statistics.TotalRevenue - statistics.TotalExpenses;
            return statistics;
        }

        public async Task<StoreStatistics> GetStoreStatisticsAsync(int mallId)
        {
            var totalStores = await _context.Stores
                .CountAsync(s => s.MallId == mallId);

            var occupiedStores = await _context.Stores
                .CountAsync(s => s.MallId == mallId && s.Renters.Any());

            var vacantStores = totalStores - occupiedStores;

            return new StoreStatistics
            {
                TotalStores = totalStores,
                OccupiedStores = occupiedStores,
                VacantStores = vacantStores,
                OccupancyRate = totalStores > 0 ? (decimal)occupiedStores / totalStores * 100 : 0
            };
        }

        public async Task<EmployeeStatistics> GetEmployeeStatisticsAsync(int mallId)
        {
            var totalEmployees = await _context.Employees
                .CountAsync(e => e.MallId == mallId);

            var activeEmployees = await _context.Employees
                .CountAsync(e => e.MallId == mallId && e.IsActive);

            //var departmentDistribution = await _context.Employees
            //    .Where(e => e.MallId == mallId)
            //    .GroupBy(e => e.Department)
            //    .Select(g => new { Department = g.Key, Count = g.Count() })
            //    .ToDictionaryAsync(x => x.Department, x => x.Count);

            return new EmployeeStatistics
            {
                TotalEmployees = totalEmployees,
                ActiveEmployees = activeEmployees,
                //DepartmentDistribution = departmentDistribution
            };
        }

        public async Task<RenterStatistics> GetRenterStatisticsAsync(int mallId)
        {
            var totalRenters = await _context.Renters
                .CountAsync(r => r.Stores.Where(s => s.MallId == mallId).Count() > 0);

            var activeRenters = await _context.Renters
                .CountAsync(r => r.Stores.Where(s => s.MallId == mallId).Count() > 0 && r.IsActive);

            var totalRent = await _context.StoreRentContracts
                .Where(c => c.Store.MallId == mallId)
                .SumAsync(c => c.MonthlyRent);

            return new RenterStatistics
            {
                TotalRenters = totalRenters,
                ActiveRenters = activeRenters,
                TotalRentIncome = totalRent,
                AverageRent = totalRenters > 0 ? totalRent / totalRenters : 0
            };
        }

        public async Task<ContractStatistics> GetContractStatisticsAsync(int mallId)
        {
            var totalContracts = await _context.StoreRentContracts
                .CountAsync(c => c.Store.MallId == mallId);

            var activeContracts = await _context.StoreRentContracts
                .CountAsync(c => c.Store.MallId == mallId && c.EndDate > DateTime.Now);

            var expiringContracts = await _context.StoreRentContracts
                .CountAsync(c => c.Store.MallId == mallId && 
                    c.EndDate > DateTime.Now && 
                    c.EndDate <= DateTime.Now.AddMonths(1));

            return new ContractStatistics
            {
                TotalContracts = totalContracts,
                ActiveContracts = activeContracts,
                ExpiringContracts = expiringContracts,
                RenewalRate = totalContracts > 0 ? (decimal)activeContracts / totalContracts * 100 : 0
            };
        }

        public async Task<Dictionary<string, decimal>> GetRevenueDataAsync(int mallId, DateTime startDate, DateTime endDate)
        {
            var revenueData = await _context.RentInvoices
                .Where(i => i.Store.MallId == mallId && i.InvoiceDate >= startDate && i.InvoiceDate <= endDate)
                .GroupBy(i => i.InvoiceDate.Month)
                .Select(g => new { Month = g.Key, Revenue = g.Sum(i => i.Amount) })
                .ToListAsync();

            return revenueData.ToDictionary(
                x => new DateTime(DateTime.Now.Year, x.Month, 1).ToString("MMMM"),
                x => x.Revenue
            );
        }

        public async Task<Dictionary<string, decimal>> GetOccupancyDataAsync(int mallId)
        {
            var totalStores = await _context.Stores.CountAsync(s => s.MallId == mallId);
            var occupiedStores = await _context.Stores
                .CountAsync(s => s.MallId == mallId && s.Renters.Any());

            return new Dictionary<string, decimal>
            {
                { "Occupied", occupiedStores },
                { "Vacant", totalStores - occupiedStores }
            };
        }

        public async Task<Dictionary<string, int>> GetPaymentStatusDataAsync(int mallId)
        {
            var pendingPayments = await _context.RentInvoices
                .CountAsync(i => i.Store.MallId == mallId && i.IsPending);

            var paidPayments = await _context.RentInvoices
                .CountAsync(i => i.Store.MallId == mallId && !i.IsPending);

            return new Dictionary<string, int>
            {
                { "Pending", pendingPayments },
                { "Paid", paidPayments }
            };
        }

        public async Task<Dictionary<string, decimal>> GetEmployeePerformanceDataAsync(int mallId)
        {
            var performanceData = await _context.Employees
                .Where(e => e.MallId == mallId)
                .Select(e => new { e.Name, Performance = e.EmployeeAttendance.Count(a => a.IsPresent) / (decimal)e.EmployeeAttendance.Count * 100 })
                .ToListAsync();

            return performanceData.ToDictionary(
                x => x.Name,
                x => x.Performance
            );
        }

        public async Task<LoanStatistics> GetLoanStatisticsAsync(int mallId)
        {
            var loans = await _context.EmployeeLoans
                .Include(l => l.Employee)
                .Where(l => l.Employee.MallId == mallId)
                .ToListAsync();

            var statistics = new LoanStatistics
            {
                TotalLoans = loans.Count,
                ActiveLoans = loans.Count(l => l.IsActive),
                TotalLoanAmount = loans.Sum(l => l.Amount),
                TotalPaidAmount = loans.Where(l => !l.IsActive).Sum(l => l.Amount),
                AverageLoanAmount = loans.Any() ? loans.Average(l => l.Amount) : 0
            };

            return statistics;
        }

        public async Task<DebitStatistics> GetDebitStatisticsAsync(int mallId)
        {
            var renterDebits = await _context.DebitForRenters
                .Include(r => r.Store)
                .Where(d => d.Store.MallId == mallId)
                .ToListAsync();

            var nonRenterDebits = await _context.DebitForNonRenters
                .ToListAsync();

            var allDebits = renterDebits.Concat<dynamic>(nonRenterDebits).ToList();

            var statistics = new DebitStatistics
            {
                TotalDebits = allDebits.Count,
                ActiveDebits = allDebits.Count(d => d.IsActive),
                TotalDebitAmount = allDebits.Sum(d => d.Amount),
                TotalPaidAmount = allDebits.Where(d => !d.IsActive).Sum(d => d.Amount),
                AverageDebitAmount = allDebits.Count > 0 ? (decimal)allDebits.Average(d => d.Amount) : 0
            };

            return statistics;
        }
    }
} 