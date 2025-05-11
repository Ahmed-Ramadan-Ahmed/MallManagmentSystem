using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MallManagmentSystem.Interfaces;
using MallManagmentSystem.Models;
using System.Collections.Generic;

namespace MallManagmentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentInvoiceController : ControllerBase
    {
        private readonly IRentInvoiceService _rentInvoiceService;

        public RentInvoiceController(IRentInvoiceService rentInvoiceService)
        {
            _rentInvoiceService = rentInvoiceService;
        }

        [HttpPost("generate-monthly")]
        public async Task<IActionResult> GenerateMonthlyInvoices()
        {
            try
            {
                await _rentInvoiceService.GenerateMonthlyInvoicesAsync();
                return Ok(new { message = "Monthly invoices generated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("generate/{storeId}")]
        public async Task<IActionResult> GenerateInvoiceForStore(int storeId, [FromQuery] DateTime invoiceDate)
        {
            try
            {
                var invoice = await _rentInvoiceService.GenerateInvoiceForStoreAsync(storeId, invoiceDate);
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RentInvoice>> GetInvoice(int id)
        {
            try
            {
                var invoice = await _rentInvoiceService.GetInvoiceByIdAsync(id);
                if (invoice == null)
                    return NotFound(new { error = $"Invoice {id} not found" });

                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("store/{storeId}")]
        public async Task<ActionResult<List<RentInvoice>>> GetInvoicesByStore(int storeId)
        {
            try
            {
                var invoices = await _rentInvoiceService.GetInvoicesByStoreAsync(storeId);
                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("renter/{renterId}")]
        public async Task<ActionResult<List<RentInvoice>>> GetInvoicesByRenter(int renterId)
        {
            try
            {
                var invoices = await _rentInvoiceService.GetInvoicesByRenterAsync(renterId);
                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("overdue")]
        public async Task<ActionResult<List<RentInvoice>>> GetOverdueInvoices()
        {
            try
            {
                var invoices = await _rentInvoiceService.GetOverdueInvoicesAsync();
                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("pending")]
        public async Task<ActionResult<List<RentInvoice>>> GetPendingInvoices()
        {
            try
            {
                var invoices = await _rentInvoiceService.GetPendingInvoicesAsync();
                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
} 