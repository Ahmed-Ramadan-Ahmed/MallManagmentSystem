using Microsoft.AspNetCore.Mvc;
using MallManagmentSystem.Interfaces;
using MallManagmentSystem.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MallManagmentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RenterController : ControllerBase
    {
        private readonly IRenterService _renterService;
        private readonly ILogger<RenterController> _logger;

        public RenterController(IRenterService renterService, ILogger<RenterController> logger)
        {
            _renterService = renterService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Renter>>> GetAllRenters()
        {
            try
            {
                var renters = await _renterService.GetAllRentersAsync();
                return Ok(renters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all renters");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Renter>> GetRenterById(int id)
        {
            try
            {
                var renter = await _renterService.GetRenterByIdAsync(id);
                if (renter == null)
                    return NotFound($"Renter with ID {id} not found");

                return Ok(renter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting renter with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Renter>> CreateRenter(Renter renter)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var success = await _renterService.CreateRenterAsync(renter);
                if (!success)
                    return BadRequest("Failed to create renter");

                return CreatedAtAction(nameof(GetRenterById), new { id = renter.Id }, renter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating renter");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRenter(int id, Renter renter)
        {
            try
            {
                if (id != renter.Id)
                    return BadRequest("ID mismatch");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var success = await _renterService.UpdateRenterAsync(id, renter);
                if (!success)
                    return NotFound($"Renter with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating renter with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRenter(int id)
        {
            try
            {
                var success = await _renterService.DeleteRenterAsync(id);
                if (!success)
                    return NotFound($"Renter with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting renter with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{renterId}/stores")]
        public async Task<ActionResult<IEnumerable<Store>>> GetRenterStores(int renterId)
        {
            try
            {
                var stores = await _renterService.GetStoresByRenterIdAsync(renterId);
                return Ok(stores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting stores for renter with ID {renterId}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{renterId}/stores/{storeId}")]
        public async Task<IActionResult> AddStoreToRenter(int renterId, int storeId)
        {
            try
            {
                var success = await _renterService.AddStoreToRenterAsync(renterId, storeId);
                if (!success)
                    return BadRequest("Failed to add store to renter");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while adding store {storeId} to renter {renterId}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{renterId}/stores/{storeId}")]
        public async Task<IActionResult> RemoveStoreFromRenter(int renterId, int storeId)
        {
            try
            {
                var success = await _renterService.RemoveStoreFromRenterAsync(renterId, storeId);
                if (!success)
                    return BadRequest("Failed to remove store from renter");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while removing store {storeId} from renter {renterId}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{renterId}/contracts")]
        public async Task<IActionResult> AddRentContract(int renterId, StoreRentContract contract)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var success = await _renterService.AddRentContractAsync(renterId, contract);
                if (!success)
                    return BadRequest("Failed to add rent contract");

                return CreatedAtAction(nameof(GetRenterContracts), new { renterId }, contract);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while adding rent contract for renter {renterId}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{renterId}/contracts/{contractId}")]
        public async Task<IActionResult> UpdateRentContract(int renterId, int contractId, StoreRentContract contract)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var success = await _renterService.UpdateRentContractAsync(renterId, contractId, contract);
                if (!success)
                    return NotFound($"Contract with ID {contractId} not found for renter {renterId}");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating rent contract {contractId} for renter {renterId}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{renterId}/contracts")]
        public async Task<ActionResult<IEnumerable<StoreRentContract>>> GetRenterContracts(int renterId)
        {
            try
            {
                var contracts = await _renterService.GetRentContractsByRenterIdAsync(renterId);
                return Ok(contracts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting contracts for renter {renterId}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{renterId}/stores/{storeId}/penalties")]
        public async Task<IActionResult> AddStorePenalty(int renterId, int storeId, StorePenalty penalty)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var success = await _renterService.AddStorePenaltyAsync(renterId, storeId, penalty);
                if (!success)
                    return BadRequest("Failed to add store penalty");

                return CreatedAtAction(nameof(GetRenterPenalties), new { renterId }, penalty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while adding penalty for store {storeId} and renter {renterId}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{renterId}/stores/{storeId}/penalties/{penaltyId}")]
        public async Task<IActionResult> RemoveStorePenalty(int renterId, int storeId, int penaltyId)
        {
            try
            {
                var success = await _renterService.RemoveStorePenaltyAsync(renterId, storeId, penaltyId);
                if (!success)
                    return NotFound($"Penalty with ID {penaltyId} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while removing penalty {penaltyId} for store {storeId} and renter {renterId}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{renterId}/penalties")]
        public async Task<ActionResult<IEnumerable<StorePenalty>>> GetRenterPenalties(int renterId)
        {
            try
            {
                var penalties = await _renterService.GetStorePenaltiesByRenterIdAsync(renterId);
                return Ok(penalties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting penalties for renter {renterId}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("{renterId}/payments")]
        public async Task<IActionResult> AddRenterPayment(int renterId, RentInvoice invoice)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var success = await _renterService.AddRenterPaymentAsync(renterId, invoice);
                if (!success)
                    return BadRequest("Failed to add renter payment");

                return CreatedAtAction(nameof(GetRenterInvoices), new { renterId }, invoice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while adding payment for renter {renterId}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{renterId}/invoices")]
        public async Task<ActionResult<IEnumerable<RentInvoice>>> GetRenterInvoices(
            int renterId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var invoices = await _renterService.GetRenterInvoicesByRenterIdAsync(renterId, startDate, endDate);
                return Ok(invoices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting invoices for renter {renterId}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
} 