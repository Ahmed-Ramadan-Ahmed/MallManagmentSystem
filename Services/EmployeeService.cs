using MallManagmentSystem.Data;
using MallManagmentSystem.Interfaces;
using MallManagmentSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace MallManagmentSystem.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly MallDBContext _context;

        public EmployeeService(MallDBContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Include(e => e.Mall)
                .Include(e => e.EmploymentContracts)
                .Include(e => e.EmployeeLoans)
                .Include(e => e.EmployeeAttendance)
                .Include(e => e.WorkPenaltyDeductions)
                .ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Mall)
                .Include(e => e.EmploymentContracts)
                .Include(e => e.EmployeeLoans)
                .Include(e => e.EmployeeAttendance)
                .Include(e => e.WorkPenaltyDeductions)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> CreateEmployeeAsync(Employee employee)
        {
            try
            {
                employee.CreatedAt = DateTime.UtcNow;
                employee.UpdatedAt = DateTime.UtcNow;
                await _context.Employees.AddAsync(employee);
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

        public async Task<bool> UpdateEmployeeAsync(int id, Employee employee)
        {
            try
            {
                var existingEmployee = await _context.Employees.FindAsync(id);
                if (existingEmployee == null)
                    return false;

                employee.UpdatedAt = DateTime.UtcNow;
                _context.Entry(existingEmployee).CurrentValues.SetValues(employee);

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(existingEmployee);
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

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            try
            {
                var employee = await _context.Employees
                    .Include(e => e.Mall)
                    .Include(e => e.EmploymentContracts)
                    .Include(e => e.EmployeeLoans)
                    .Include(e => e.EmployeeAttendance)
                    .Include(e => e.WorkPenaltyDeductions)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (employee == null)
                    return false;

                // Check if employee has active contracts, penalties, or loans
                //if (employee.Contracts.Any() || employee.Penalties.Any() || employee.Loans.Any())
                //    return false;

                _context.Employees.Remove(employee);

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(employee);
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

        public async Task<List<Employee>> GetEmployeesByMallIdAsync(int mallId)
        {
            return await _context.Employees
                .Include(e => e.Mall)
                .Include(e => e.EmploymentContracts)
                .Include(e => e.EmployeeLoans)
                .Include(e => e.EmployeeAttendance)
                .Include(e => e.WorkPenaltyDeductions)
                .Where(e => e.MallId == mallId)
                .ToListAsync();
        }

        public async Task<bool> AssignEmployeeToMallAsync(int employeeId, int mallId)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(employeeId);
                var mall = await _context.Malls.FindAsync(mallId);

                if (employee == null || mall == null)
                    return false;

                employee.MallId = mallId;
                employee.UpdatedAt = DateTime.UtcNow;

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(employee);
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

        public async Task<bool> RemoveEmployeeFromMallAsync(int employeeId)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(employeeId);
                if (employee == null)
                    return false;

                employee.MallId = 0;
                employee.UpdatedAt = DateTime.UtcNow;

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(employee);
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

        public async Task<bool> AddEmploymentContractAsync(int employeeId, EmploymentContract contract)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(employeeId);
                if (employee == null)
                    return false;

                contract.EmployeeId = employeeId;
                contract.StartDate = DateTime.UtcNow;
                contract.UpdatedAt = DateTime.UtcNow;

                await _context.EmploymentContracts.AddAsync(contract);

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(contract);
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

        public async Task<bool> UpdateEmploymentContractAsync(int employeeId, int contractId, EmploymentContract contract)
        {
            try
            {
                var existingContract = await _context.EmploymentContracts
                    .FirstOrDefaultAsync(c => c.Id == contractId && c.EmployeeId == employeeId);

                if (existingContract == null)
                    return false;

                contract.UpdatedAt = DateTime.UtcNow;
                _context.Entry(existingContract).CurrentValues.SetValues(contract);

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(existingContract);
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

        public async Task<List<EmploymentContract>> GetEmploymentContractsByEmployeeIdAsync(int employeeId)
        {
            return await _context.EmploymentContracts
                .Include(c => c.Employee)
                .Where(c => c.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<bool> AddWorkPenaltyDeductionAsync(int employeeId, WorkPenaltyDeduction penalty)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(employeeId);
                if (employee == null)
                    return false;

                penalty.EmployeeId = employeeId;
                penalty.CreatedAt = DateTime.UtcNow;
                penalty.UpdatedAt = DateTime.UtcNow;

                await _context.WorkPenaltyDeductions.AddAsync(penalty);

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(penalty);
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

        public async Task<bool> RemoveWorkPenaltyDeductionAsync(int employeeId, int penaltyId)
        {
            try
            {
                var penalty = await _context.WorkPenaltyDeductions
                    .FirstOrDefaultAsync(p => p.Id == penaltyId && p.EmployeeId == employeeId);

                if (penalty == null)
                    return false;

                _context.WorkPenaltyDeductions.Remove(penalty);

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(penalty);
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

        public async Task<List<WorkPenaltyDeduction>> GetWorkPenaltyDeductionsByEmployeeIdAsync(int employeeId)
        {
            return await _context.WorkPenaltyDeductions
                .Include(p => p.Employee)
                .Where(p => p.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<bool> AddEmployeeLoanAsync(int employeeId, EmployeeLoan loan)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(employeeId);
                if (employee == null)
                    return false;

                loan.EmployeeId = employeeId;
                loan.CreatedAt = DateTime.UtcNow;
                loan.UpdatedAt = DateTime.UtcNow;

                await _context.EmployeeLoans.AddAsync(loan);

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(loan);
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

        public async Task<bool> UpdateEmployeeLoanAsync(int employeeId, int loanId, EmployeeLoan loan)
        {
            try
            {
                var existingLoan = await _context.EmployeeLoans
                    .FirstOrDefaultAsync(l => l.Id == loanId && l.EmployeeId == employeeId);

                if (existingLoan == null)
                    return false;

                loan.UpdatedAt = DateTime.UtcNow;
                _context.Entry(existingLoan).CurrentValues.SetValues(loan);

                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    var entry = _context.Entry(existingLoan);
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

        public async Task<List<EmployeeLoan>> GetEmployeeLoansByEmployeeIdAsync(int employeeId)
        {
            return await _context.EmployeeLoans
                .Include(l => l.Employee)
                .Where(l => l.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<bool> RecordAttendanceAsync(int employeeId, Attendance attendance)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(employeeId);
                if (employee == null)
                    return false;

                attendance.EmployeeId = employeeId;
                attendance.CreatedAt = DateTime.UtcNow;
                attendance.UpdatedAt = DateTime.UtcNow;

                await _context.Attendances.AddAsync(attendance);

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

        public async Task<List<Attendance>> GetAttendanceByEmployeeIdAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            return await _context.Attendances
                .Include(a => a.Employee)
                .Where(a => a.EmployeeId == employeeId && a.Date >= startDate && a.Date <= endDate)
                .ToListAsync();
        }
    }
} 