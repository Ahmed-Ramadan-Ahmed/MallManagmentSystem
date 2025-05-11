using Microsoft.AspNetCore.Mvc;
using MallManagmentSystem.Interfaces;
using MallManagmentSystem.Models;

namespace MallManagmentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Attendance>>> GetAllAttendance()
        {
            var attendance = await _attendanceService.GetAllAttendanceAsync();
            return Ok(attendance);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Attendance>> GetAttendanceById(int id)
        {
            var attendance = await _attendanceService.GetAttendanceByIdAsync(id);
            if (attendance == null)
                return NotFound();

            return Ok(attendance);
        }

        [HttpPost]
        public async Task<ActionResult<Attendance>> CreateAttendance(Attendance attendance)
        {
            var result = await _attendanceService.CreateAttendanceAsync(attendance);
            if (!result)
                return BadRequest("Failed to create attendance record");

            return CreatedAtAction(nameof(GetAttendanceById), new { id = attendance.Id }, attendance);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAttendance(int id, Attendance attendance)
        {
            if (id != attendance.Id)
                return BadRequest();

            var result = await _attendanceService.UpdateAttendanceAsync(id, attendance);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendance(int id)
        {
            var result = await _attendanceService.DeleteAttendanceAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<List<Attendance>>> GetAttendanceByEmployeeId(int employeeId)
        {
            var attendance = await _attendanceService.GetAttendanceByEmployeeIdAsync(employeeId);
            return Ok(attendance);
        }

        [HttpGet("date-range")]
        public async Task<ActionResult<List<Attendance>>> GetAttendanceByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var attendance = await _attendanceService.GetAttendanceByDateRangeAsync(startDate, endDate);
            return Ok(attendance);
        }

        [HttpGet("mall/{mallId}")]
        public async Task<ActionResult<List<Attendance>>> GetAttendanceByMallId(int mallId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var attendance = await _attendanceService.GetAttendanceByMallIdAsync(mallId, startDate, endDate);
            return Ok(attendance);
        }

        [HttpGet("employee/{employeeId}/summary")]
        public async Task<ActionResult<AttendanceSummary>> GetAttendanceSummaryByEmployeeId(int employeeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var summary = await _attendanceService.GetAttendanceSummaryByEmployeeIdAsync(employeeId, startDate, endDate);
            return Ok(summary);
        }

        [HttpGet("mall/{mallId}/summary")]
        public async Task<ActionResult<AttendanceSummary>> GetAttendanceSummaryByMallId(int mallId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var summary = await _attendanceService.GetAttendanceSummaryByMallIdAsync(mallId, startDate, endDate);
            return Ok(summary);
        }
    }
} 