using MallManagmentSystem.Data;
using MallManagmentSystem.Interfaces;
using MallManagmentSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace MallManagmentSystem.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly MallDBContext _context;

        public AttendanceService(MallDBContext context)
        {
            _context = context;
        }

        public async Task<List<Attendance>> GetAllAttendanceAsync()
        {
            return await _context.Attendances
                .Include(a => a.Employee)
                .ThenInclude(e => e.Mall)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }

        public async Task<Attendance> GetAttendanceByIdAsync(int id)
        {
            return await _context.Attendances
                .Include(a => a.Employee)
                .ThenInclude(e => e.Mall)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> CreateAttendanceAsync(Attendance attendance)
        {
            try
            {
                // Validate employee exists
                var employee = await _context.Employees.FindAsync(attendance.EmployeeId);
                if (employee == null)
                    return false;

                // Check for duplicate attendance record
                var existingAttendance = await _context.Attendances
                    .FirstOrDefaultAsync(a => a.EmployeeId == attendance.EmployeeId && 
                                            a.Date.Date == attendance.Date.Date);
                if (existingAttendance != null)
                    return false;

                attendance.CreatedAt = DateTime.UtcNow;
                attendance.UpdatedAt = DateTime.UtcNow;
                await _context.Attendances.AddAsync(attendance);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAttendanceAsync(int id, Attendance attendance)
        {
            try
            {
                var existingAttendance = await _context.Attendances.FindAsync(id);
                if (existingAttendance == null)
                    return false;

                // Validate employee exists
                var employee = await _context.Employees.FindAsync(attendance.EmployeeId);
                if (employee == null)
                    return false;

                // Check for duplicate attendance record (excluding current record)
                var duplicateAttendance = await _context.Attendances
                    .FirstOrDefaultAsync(a => a.EmployeeId == attendance.EmployeeId && 
                                            a.Date.Date == attendance.Date.Date &&
                                            a.Id != id);
                if (duplicateAttendance != null)
                    return false;

                attendance.UpdatedAt = DateTime.UtcNow;
                _context.Entry(existingAttendance).CurrentValues.SetValues(attendance);

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(existingAttendance);
                    var databaseValues = await entry.GetDatabaseValuesAsync();

                    if (databaseValues == null)
                    {
                        return false;
                    }

                    entry.OriginalValues.SetValues(databaseValues);

                    try
                    {
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAttendanceAsync(int id)
        {
            try
            {
                var attendance = await _context.Attendances.FindAsync(id);
                if (attendance == null)
                    return false;

                _context.Attendances.Remove(attendance);

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(attendance);
                    var databaseValues = await entry.GetDatabaseValuesAsync();

                    if (databaseValues == null)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Attendance>> GetAttendanceByEmployeeIdAsync(int employeeId)
        {
            return await _context.Attendances
                .Include(a => a.Employee)
                .ThenInclude(e => e.Mall)
                .Where(a => a.EmployeeId == employeeId)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }

        public async Task<List<Attendance>> GetAttendanceByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Attendances
                .Include(a => a.Employee)
                .ThenInclude(e => e.Mall)
                .Where(a => a.Date.Date >= startDate.Date && a.Date.Date <= endDate.Date)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }

        public async Task<List<Attendance>> GetAttendanceByMallIdAsync(int mallId, DateTime startDate, DateTime endDate)
        {
            return await _context.Attendances
                .Include(a => a.Employee)
                .ThenInclude(e => e.Mall)
                .Where(a => a.Employee.MallId == mallId && 
                           a.Date.Date >= startDate.Date && 
                           a.Date.Date <= endDate.Date)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }

        public async Task<AttendanceSummary> GetAttendanceSummaryByEmployeeIdAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            var attendance = await _context.Attendances
                .Where(a => a.EmployeeId == employeeId && 
                           a.Date.Date >= startDate.Date && 
                           a.Date.Date <= endDate.Date)
                .ToListAsync();

            var summary = new AttendanceSummary
            {
                TotalDays = (endDate.Date - startDate.Date).Days + 1,
                PresentDays = attendance.Count(a => a.Status == "Present"),
                AbsentDays = attendance.Count(a => a.Status == "Absent"),
                LateDays = attendance.Count(a => a.Status == "Late"),
                EarlyLeaveDays = attendance.Count(a => a.Status == "EarlyLeave")
            };

            summary.AttendancePercentage = summary.TotalDays > 0 
                ? (double)summary.PresentDays / summary.TotalDays * 100 
                : 0;

            return summary;
        }

        public async Task<AttendanceSummary> GetAttendanceSummaryByMallIdAsync(int mallId, DateTime startDate, DateTime endDate)
        {
            var attendance = await _context.Attendances
                .Include(a => a.Employee)
                .Where(a => a.Employee.MallId == mallId && 
                           a.Date.Date >= startDate.Date && 
                           a.Date.Date <= endDate.Date)
                .ToListAsync();

            var summary = new AttendanceSummary
            {
                TotalDays = (endDate.Date - startDate.Date).Days + 1,
                PresentDays = attendance.Count(a => a.Status == "Present"),
                AbsentDays = attendance.Count(a => a.Status == "Absent"),
                LateDays = attendance.Count(a => a.Status == "Late"),
                EarlyLeaveDays = attendance.Count(a => a.Status == "EarlyLeave")
            };

            summary.AttendancePercentage = summary.TotalDays > 0 
                ? (double)summary.PresentDays / summary.TotalDays * 100 
                : 0;

            return summary;
        }
    }
} 