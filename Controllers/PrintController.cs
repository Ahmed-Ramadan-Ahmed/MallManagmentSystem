using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MallManagmentSystem.Interfaces;
using System.Collections.Generic;

namespace MallManagmentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrintController : ControllerBase
    {
        private readonly IPrintService _printService;

        public PrintController(IPrintService printService)
        {
            _printService = printService;
        }

        #region Invoice Printing

        [HttpGet("invoice/{id}")]
        public async Task<IActionResult> PrintInvoice(int id)
        {
            try
            {
                var pdfBytes = await _printService.PrintRentInvoiceAsync(id);
                return File(pdfBytes, "application/pdf", $"Invoice_{id}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("invoice/{id}/with-payments")]
        public async Task<IActionResult> PrintInvoiceWithPayments(int id)
        {
            try
            {
                var pdfBytes = await _printService.PrintRentInvoiceWithPaymentsAsync(id);
                return File(pdfBytes, "application/pdf", $"Invoice_{id}_WithPayments.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("invoices")]
        public async Task<IActionResult> PrintMultipleInvoices([FromBody] List<int> invoiceIds)
        {
            try
            {
                var pdfBytes = await _printService.PrintMultipleInvoicesAsync(invoiceIds);
                return File(pdfBytes, "application/pdf", $"MultipleInvoices_{DateTime.Now:yyyyMMdd}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        #endregion

        #region Contract Printing

        [HttpGet("store-contract/{id}")]
        public async Task<IActionResult> PrintStoreContract(int id)
        {
            try
            {
                var pdfBytes = await _printService.PrintStoreContractAsync(id);
                return File(pdfBytes, "application/pdf", $"StoreContract_{id}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("employee-contract/{id}")]
        public async Task<IActionResult> PrintEmployeeContract(int id)
        {
            try
            {
                var pdfBytes = await _printService.PrintEmployeeContractAsync(id);
                return File(pdfBytes, "application/pdf", $"EmployeeContract_{id}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("contracts")]
        public async Task<IActionResult> PrintMultipleContracts([FromBody] List<int> contractIds)
        {
            try
            {
                var pdfBytes = await _printService.PrintMultipleContractsAsync(contractIds);
                return File(pdfBytes, "application/pdf", $"MultipleContracts_{DateTime.Now:yyyyMMdd}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        #endregion

        #region Statistics Reports

        [HttpGet("statistics/financial/{mallId}")]
        public async Task<IActionResult> PrintFinancialStatistics(int mallId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var pdfBytes = await _printService.PrintFinancialStatisticsAsync(mallId, startDate, endDate);
                return File(pdfBytes, "application/pdf", $"FinancialStatistics_{mallId}_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("statistics/store/{mallId}")]
        public async Task<IActionResult> PrintStoreStatistics(int mallId)
        {
            try
            {
                var pdfBytes = await _printService.PrintStoreStatisticsAsync(mallId);
                return File(pdfBytes, "application/pdf", $"StoreStatistics_{mallId}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("statistics/employee/{mallId}")]
        public async Task<IActionResult> PrintEmployeeStatistics(int mallId)
        {
            try
            {
                var pdfBytes = await _printService.PrintEmployeeStatisticsAsync(mallId);
                return File(pdfBytes, "application/pdf", $"EmployeeStatistics_{mallId}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("statistics/renter/{mallId}")]
        public async Task<IActionResult> PrintRenterStatistics(int mallId)
        {
            try
            {
                var pdfBytes = await _printService.PrintRenterStatisticsAsync(mallId);
                return File(pdfBytes, "application/pdf", $"RenterStatistics_{mallId}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("statistics/contract/{mallId}")]
        public async Task<IActionResult> PrintContractStatistics(int mallId)
        {
            try
            {
                var pdfBytes = await _printService.PrintContractStatisticsAsync(mallId);
                return File(pdfBytes, "application/pdf", $"ContractStatistics_{mallId}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        #endregion

        #region Charts

        [HttpGet("charts/revenue/{mallId}")]
        public async Task<IActionResult> GenerateRevenueChart(int mallId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var imageBytes = await _printService.GenerateRevenueChartAsync(mallId, startDate, endDate);
                return File(imageBytes, "image/png", $"RevenueChart_{mallId}_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.png");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("charts/occupancy/{mallId}")]
        public async Task<IActionResult> GenerateOccupancyChart(int mallId)
        {
            try
            {
                var imageBytes = await _printService.GenerateOccupancyChartAsync(mallId);
                return File(imageBytes, "image/png", $"OccupancyChart_{mallId}.png");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("charts/payment-status/{mallId}")]
        public async Task<IActionResult> GeneratePaymentStatusChart(int mallId)
        {
            try
            {
                var imageBytes = await _printService.GeneratePaymentStatusChartAsync(mallId);
                return File(imageBytes, "image/png", $"PaymentStatusChart_{mallId}.png");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("charts/employee-performance/{mallId}")]
        public async Task<IActionResult> GenerateEmployeePerformanceChart(int mallId)
        {
            try
            {
                var imageBytes = await _printService.GenerateEmployeePerformanceChartAsync(mallId);
                return File(imageBytes, "image/png", $"EmployeePerformanceChart_{mallId}.png");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        #endregion
    }
} 