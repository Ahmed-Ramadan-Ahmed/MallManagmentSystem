using Microsoft.AspNetCore.Mvc;
using MallManagmentSystem.Interfaces;
using MallManagmentSystem.Models;

namespace MallManagmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("financial/{mallId}")]
        public async Task<ActionResult<FinancialStatistics>> GetFinancialStatistics(int mallId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var statistics = await _statisticsService.GetFinancialStatisticsAsync(mallId, startDate, endDate);
            return Ok(statistics);
        }

        [HttpGet("stores/{mallId}")]
        public async Task<ActionResult<StoreStatistics>> GetStoreStatistics(int mallId)
        {
            var statistics = await _statisticsService.GetStoreStatisticsAsync(mallId);
            return Ok(statistics);
        }

        [HttpGet("employees/{mallId}")]
        public async Task<ActionResult<EmployeeStatistics>> GetEmployeeStatistics(int mallId)
        {
            var statistics = await _statisticsService.GetEmployeeStatisticsAsync(mallId);
            return Ok(statistics);
        }

        [HttpGet("renters/{mallId}")]
        public async Task<ActionResult<RenterStatistics>> GetRenterStatistics(int mallId)
        {
            var statistics = await _statisticsService.GetRenterStatisticsAsync(mallId);
            return Ok(statistics);
        }

        [HttpGet("loans/{mallId}")]
        public async Task<ActionResult<LoanStatistics>> GetLoanStatistics(int mallId)
        {
            var statistics = await _statisticsService.GetLoanStatisticsAsync(mallId);
            return Ok(statistics);
        }

        [HttpGet("debits/{mallId}")]
        public async Task<ActionResult<DebitStatistics>> GetDebitStatistics(int mallId)
        {
            var statistics = await _statisticsService.GetDebitStatisticsAsync(mallId);
            return Ok(statistics);
        }

        [HttpGet("contracts/{mallId}")]
        public async Task<ActionResult<ContractStatistics>> GetContractStatistics(int mallId)
        {
            var statistics = await _statisticsService.GetContractStatisticsAsync(mallId);
            return Ok(statistics);
        }

        [HttpGet("revenue/{mallId}")]
        public async Task<ActionResult<Dictionary<string,decimal>>> GetRevenueData(int mallId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var data = await _statisticsService.GetRevenueDataAsync(mallId, startDate, endDate);
            return Ok(data);
        }

        [HttpGet("occupancy/{mallId}")]
        public async Task<ActionResult<Dictionary<string, decimal>>> GetOccupancyData(int mallId)
        {
            var data = await _statisticsService.GetOccupancyDataAsync(mallId);
            var totalStores = await _statisticsService.GetStoreStatisticsAsync(mallId);
            return Ok(data);
        }

        [HttpGet("payments/{mallId}")]
        public async Task<ActionResult<Dictionary<string, decimal>>> GetPaymentStatusData(int mallId)
        {
            var data = await _statisticsService.GetPaymentStatusDataAsync(mallId);
            var overduePayments = await _statisticsService.GetRenterStatisticsAsync(mallId);
            return Ok(data);
        }

        [HttpGet("performance/{mallId}")]
        public async Task<ActionResult<Dictionary<string, decimal>>> GetEmployeePerformanceData(int mallId)
        {
            var data = await _statisticsService.GetEmployeePerformanceDataAsync(mallId);
            return Ok(data);
        }
    }
} 