using MallManagmentSystem.Models;

namespace MallManagmentSystem.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<bool> CreateEmployeeAsync(Employee employee);
        Task<bool> UpdateEmployeeAsync(int id, Employee employee);
        Task<bool> DeleteEmployeeAsync(int id);
        Task<List<Employee>> GetEmployeesByMallIdAsync(int mallId);
        Task<bool> AssignEmployeeToMallAsync(int employeeId, int mallId);
        Task<bool> RemoveEmployeeFromMallAsync(int employeeId);
        Task<bool> AddEmploymentContractAsync(int employeeId, EmploymentContract contract);
        Task<bool> UpdateEmploymentContractAsync(int employeeId, int contractId, EmploymentContract contract);
        Task<List<EmploymentContract>> GetEmploymentContractsByEmployeeIdAsync(int employeeId);
        Task<bool> AddWorkPenaltyDeductionAsync(int employeeId, WorkPenaltyDeduction penalty);
        Task<bool> RemoveWorkPenaltyDeductionAsync(int employeeId, int penaltyId);
        Task<List<WorkPenaltyDeduction>> GetWorkPenaltyDeductionsByEmployeeIdAsync(int employeeId);
        Task<bool> AddEmployeeLoanAsync(int employeeId, EmployeeLoan loan);
        Task<bool> UpdateEmployeeLoanAsync(int employeeId, int loanId, EmployeeLoan loan);
        Task<List<EmployeeLoan>> GetEmployeeLoansByEmployeeIdAsync(int employeeId);
        Task<bool> RecordAttendanceAsync(int employeeId, Attendance attendance);
        Task<List<Attendance>> GetAttendanceByEmployeeIdAsync(int employeeId, DateTime startDate, DateTime endDate);
    }
} 