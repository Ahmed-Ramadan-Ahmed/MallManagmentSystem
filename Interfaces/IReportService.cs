using MallManagmentSystem.Models;

namespace MallManagmentSystem.Interfaces
{
    public interface IReportService
    {
        Task<MallReport> GenerateMallReportAsync(int mallId, DateTime startDate, DateTime endDate);
        Task<StoreReport> GenerateStoreReportAsync(int storeId, DateTime startDate, DateTime endDate);
        Task<RenterReport> GenerateRenterReportAsync(int renterId, DateTime startDate, DateTime endDate);
        Task<EmployeeReport> GenerateEmployeeReportAsync(int employeeId, DateTime startDate, DateTime endDate);
        Task<FinancialReport> GenerateFinancialReportAsync(int mallId, DateTime startDate, DateTime endDate);
        Task<OccupancyReport> GenerateOccupancyReportAsync(int mallId, DateTime startDate, DateTime endDate);
        Task<PerformanceReport> GeneratePerformanceReportAsync(int mallId, DateTime startDate, DateTime endDate);
    }

    public class MallReport
    {
        public int TotalStores { get; set; }
        public int ActiveStores { get; set; }
        public int TotalEmployees { get; set; }
        public int TotalRenters { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetProfit { get; set; }
        public double OccupancyRate { get; set; }
        public List<StoreSummary> StoreSummaries { get; set; }
        public List<EmployeeSummary> EmployeeSummaries { get; set; }
    }

    public class StoreReport
    {
        public string StoreName { get; set; }
        public string RenterName { get; set; }
        public decimal TotalRent { get; set; }
        public decimal TotalPenalties { get; set; }
        public decimal TotalPayments { get; set; }
        public decimal OutstandingBalance { get; set; }
        public List<PaymentHistory> PaymentHistory { get; set; }
        public List<PenaltyHistory> PenaltyHistory { get; set; }
    }

    public class RenterReport
    {
        public string RenterName { get; set; }
        public int TotalStores { get; set; }
        public decimal TotalRent { get; set; }
        public decimal TotalPenalties { get; set; }
        public decimal TotalPayments { get; set; }
        public decimal OutstandingBalance { get; set; }
        public List<StoreSummary> StoreSummaries { get; set; }
        public List<PaymentHistory> PaymentHistory { get; set; }
    }

    public class EmployeeReport
    {
        public string EmployeeName { get; set; }
        public string Position { get; set; }
        public int TotalDaysWorked { get; set; }
        public int AbsentDays { get; set; }
        public int LateDays { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetSalary { get; set; }
        public List<AttendanceRecord> AttendanceHistory { get; set; }
    }

    public class FinancialReport
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetProfit { get; set; }
        public List<RevenueBreakdown> RevenueBreakdown { get; set; }
        public List<ExpenseBreakdown> ExpenseBreakdown { get; set; }
        public List<MonthlySummary> MonthlySummaries { get; set; }
    }

    public class OccupancyReport
    {
        public int TotalStores { get; set; }
        public int OccupiedStores { get; set; }
        public int VacantStores { get; set; }
        public double OccupancyRate { get; set; }
        public List<StoreStatus> StoreStatuses { get; set; }
        public List<MonthlyOccupancy> MonthlyOccupancy { get; set; }
    }

    public class PerformanceReport
    {
        public decimal RevenueGrowth { get; set; }
        public decimal OccupancyGrowth { get; set; }
        public decimal CustomerSatisfaction { get; set; }
        public List<KeyPerformanceIndicator> KPIs { get; set; }
        public List<MonthlyPerformance> MonthlyPerformance { get; set; }
    }

    // Supporting classes for report data
    public class StoreSummary
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string Status { get; set; }
        public decimal MonthlyRent { get; set; }
        public decimal OutstandingBalance { get; set; }
    }

    public class EmployeeSummary
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Position { get; set; }
        public decimal MonthlySalary { get; set; }
        public double AttendanceRate { get; set; }
    }

    public class PaymentHistory
    {
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentType { get; set; }
        public string Status { get; set; }
    }

    public class PenaltyHistory
    {
        public DateTime PenaltyDate { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
    }

    public class AttendanceRecord
    {
        public DateTime Date { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string Status { get; set; }
    }

    public class RevenueBreakdown
    {
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public double Percentage { get; set; }
    }

    public class ExpenseBreakdown
    {
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public double Percentage { get; set; }
    }

    public class MonthlySummary
    {
        public DateTime Month { get; set; }
        public decimal Revenue { get; set; }
        public decimal Expenses { get; set; }
        public decimal Profit { get; set; }
    }

    public class StoreStatus
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string Status { get; set; }
        public DateTime LastOccupied { get; set; }
    }

    public class MonthlyOccupancy
    {
        public DateTime Month { get; set; }
        public int TotalStores { get; set; }
        public int OccupiedStores { get; set; }
        public double OccupancyRate { get; set; }
    }

    public class KeyPerformanceIndicator
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Target { get; set; }
        public double Achievement { get; set; }
    }

    public class MonthlyPerformance
    {
        public DateTime Month { get; set; }
        public decimal Revenue { get; set; }
        public decimal Occupancy { get; set; }
        public decimal Satisfaction { get; set; }
    }
} 