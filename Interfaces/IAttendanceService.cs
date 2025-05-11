using MallManagmentSystem.Models;

namespace MallManagmentSystem.Interfaces
{
    public interface IAttendanceService
    {
        Task<List<Attendance>> GetAllAttendanceAsync();
        Task<Attendance> GetAttendanceByIdAsync(int id);
        Task<bool> CreateAttendanceAsync(Attendance attendance);
        Task<bool> UpdateAttendanceAsync(int id, Attendance attendance);
        Task<bool> DeleteAttendanceAsync(int id);
        Task<List<Attendance>> GetAttendanceByEmployeeIdAsync(int employeeId);
        Task<List<Attendance>> GetAttendanceByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<Attendance>> GetAttendanceByMallIdAsync(int mallId, DateTime startDate, DateTime endDate);
        Task<AttendanceSummary> GetAttendanceSummaryByEmployeeIdAsync(int employeeId, DateTime startDate, DateTime endDate);
        Task<AttendanceSummary> GetAttendanceSummaryByMallIdAsync(int mallId, DateTime startDate, DateTime endDate);
    }

    public class AttendanceSummary
    {
        public int TotalDays { get; set; }
        public int PresentDays { get; set; }
        public int AbsentDays { get; set; }
        public int LateDays { get; set; }
        public int EarlyLeaveDays { get; set; }
        public double AttendancePercentage { get; set; }
    }
} 