using Microsoft.AspNetCore.Mvc;
using MallManagmentSystem.Interfaces;
using MallManagmentSystem.Models;

namespace MallManagmentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            var result = await _employeeService.CreateEmployeeAsync(employee);
            if (!result)
                return BadRequest("Failed to create employee");

            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
                return BadRequest();

            var result = await _employeeService.UpdateEmployeeAsync(id, employee);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var result = await _employeeService.DeleteEmployeeAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("mall/{mallId}")]
        public async Task<ActionResult<List<Employee>>> GetEmployeesByMallId(int mallId)
        {
            var employees = await _employeeService.GetEmployeesByMallIdAsync(mallId);
            return Ok(employees);
        }

        [HttpPost("{employeeId}/mall/{mallId}")]
        public async Task<IActionResult> AssignEmployeeToMall(int employeeId, int mallId)
        {
            var result = await _employeeService.AssignEmployeeToMallAsync(employeeId, mallId);
            if (!result)
                return BadRequest("Failed to assign employee to mall");

            return NoContent();
        }

        [HttpDelete("{employeeId}/mall")]
        public async Task<IActionResult> RemoveEmployeeFromMall(int employeeId)
        {
            var result = await _employeeService.RemoveEmployeeFromMallAsync(employeeId);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{employeeId}/contracts")]
        public async Task<ActionResult<EmploymentContract>> AddEmploymentContract(int employeeId, EmploymentContract contract)
        {
            var result = await _employeeService.AddEmploymentContractAsync(employeeId, contract);
            if (!result)
                return BadRequest("Failed to add employment contract");

            return CreatedAtAction(nameof(GetEmployeeById), new { id = employeeId }, contract);
        }

        [HttpPut("{employeeId}/contracts/{contractId}")]
        public async Task<IActionResult> UpdateEmploymentContract(int employeeId, int contractId, EmploymentContract contract)
        {
            if (contractId != contract.Id)
                return BadRequest();

            var result = await _employeeService.UpdateEmploymentContractAsync(employeeId, contractId, contract);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{employeeId}/contracts")]
        public async Task<ActionResult<List<EmploymentContract>>> GetEmploymentContractsByEmployeeId(int employeeId)
        {
            var contracts = await _employeeService.GetEmploymentContractsByEmployeeIdAsync(employeeId);
            return Ok(contracts);
        }

        [HttpPost("{employeeId}/penalties")]
        public async Task<ActionResult<WorkPenaltyDeduction>> AddWorkPenaltyDeduction(int employeeId, WorkPenaltyDeduction penalty)
        {
            var result = await _employeeService.AddWorkPenaltyDeductionAsync(employeeId, penalty);
            if (!result)
                return BadRequest("Failed to add work penalty deduction");

            return CreatedAtAction(nameof(GetEmployeeById), new { id = employeeId }, penalty);
        }

        [HttpDelete("{employeeId}/penalties/{penaltyId}")]
        public async Task<IActionResult> RemoveWorkPenaltyDeduction(int employeeId, int penaltyId)
        {
            var result = await _employeeService.RemoveWorkPenaltyDeductionAsync(employeeId, penaltyId);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{employeeId}/penalties")]
        public async Task<ActionResult<List<WorkPenaltyDeduction>>> GetWorkPenaltyDeductionsByEmployeeId(int employeeId)
        {
            var penalties = await _employeeService.GetWorkPenaltyDeductionsByEmployeeIdAsync(employeeId);
            return Ok(penalties);
        }

        [HttpPost("{employeeId}/loans")]
        public async Task<ActionResult<EmployeeLoan>> AddEmployeeLoan(int employeeId, EmployeeLoan loan)
        {
            var result = await _employeeService.AddEmployeeLoanAsync(employeeId, loan);
            if (!result)
                return BadRequest("Failed to add employee loan");

            return CreatedAtAction(nameof(GetEmployeeById), new { id = employeeId }, loan);
        }

        [HttpPut("{employeeId}/loans/{loanId}")]
        public async Task<IActionResult> UpdateEmployeeLoan(int employeeId, int loanId, EmployeeLoan loan)
        {
            if (loanId != loan.Id)
                return BadRequest();

            var result = await _employeeService.UpdateEmployeeLoanAsync(employeeId, loanId, loan);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{employeeId}/loans")]
        public async Task<ActionResult<List<EmployeeLoan>>> GetEmployeeLoansByEmployeeId(int employeeId)
        {
            var loans = await _employeeService.GetEmployeeLoansByEmployeeIdAsync(employeeId);
            return Ok(loans);
        }

        [HttpPost("{employeeId}/attendance")]
        public async Task<ActionResult<Attendance>> RecordAttendance(int employeeId, Attendance attendance)
        {
            var result = await _employeeService.RecordAttendanceAsync(employeeId, attendance);
            if (!result)
                return BadRequest("Failed to record attendance");

            return CreatedAtAction(nameof(GetEmployeeById), new { id = employeeId }, attendance);
        }

        [HttpGet("{employeeId}/attendance")]
        public async Task<ActionResult<List<Attendance>>> GetAttendanceByEmployeeId(int employeeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var attendance = await _employeeService.GetAttendanceByEmployeeIdAsync(employeeId, startDate, endDate);
            return Ok(attendance);
        }
    }
} 