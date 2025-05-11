using Microsoft.AspNetCore.Mvc;
using MallManagmentSystem.Interfaces;
using MallManagmentSystem.Models;

namespace MallManagmentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Store>>> GetAllStores()
        {
            var stores = await _storeService.GetAllStoresAsync();
            return Ok(stores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Store>> GetStoreById(int id)
        {
            var store = await _storeService.GetStoreByIdAsync(id);
            if (store == null)
                return NotFound();

            return Ok(store);
        }

        [HttpPost]
        public async Task<ActionResult<Store>> CreateStore(Store store)
        {
            var result = await _storeService.CreateStoreAsync(store);
            if (!result)
                return BadRequest("Failed to create store");

            return CreatedAtAction(nameof(GetStoreById), new { id = store.Id }, store);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStore(int id, Store store)
        {
            if (id != store.Id)
                return BadRequest();

            var result = await _storeService.UpdateStoreAsync(id, store);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(int id)
        {
            var result = await _storeService.DeleteStoreAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("mall/{mallId}")]
        public async Task<ActionResult<List<Store>>> GetStoresByMallId(int mallId)
        {
            var stores = await _storeService.GetStoresByMallIdAsync(mallId);
            return Ok(stores);
        }

        [HttpGet("renter/{renterId}")]
        public async Task<ActionResult<List<Store>>> GetStoresByRenterId(int renterId)
        {
            var stores = await _storeService.GetStoresByRenterIdAsync(renterId);
            return Ok(stores);
        }

        [HttpPost("{storeId}/renter/{renterId}")]
        public async Task<IActionResult> AssignRenterToStore(int storeId, int renterId)
        {
            var result = await _storeService.AssignRenterToStoreAsync(storeId, renterId);
            if (!result)
                return BadRequest("Failed to assign renter to store");

            return NoContent();
        }

        [HttpDelete("{storeId}/renter")]
        public async Task<IActionResult> RemoveRenterFromStore(int storeId)
        {
            var result = await _storeService.RemoveRenterFromStoreAsync(storeId);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{storeId}/penalties")]
        public async Task<ActionResult<StorePenalty>> AddPenaltyToStore(int storeId, StorePenalty penalty)
        {
            var result = await _storeService.AddPenaltyToStoreAsync(storeId, penalty);
            if (!result)
                return BadRequest("Failed to add penalty to store");

            return CreatedAtAction(nameof(GetStoreById), new { id = storeId }, penalty);
        }

        [HttpDelete("{storeId}/penalties/{penaltyId}")]
        public async Task<IActionResult> RemovePenaltyFromStore(int storeId, int penaltyId)
        {
            var result = await _storeService.RemovePenaltyFromStoreAsync(storeId, penaltyId);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{storeId}/penalties")]
        public async Task<ActionResult<List<StorePenalty>>> GetPenaltiesByStoreId(int storeId)
        {
            var penalties = await _storeService.GetPenaltiesByStoreIdAsync(storeId);
            return Ok(penalties);
        }

        [HttpPut("{storeId}/penalties/{penaltyId}")]
        public async Task<IActionResult> UpdatePenalty(int storeId, int penaltyId, StorePenalty penalty)
        {
            if (penaltyId != penalty.Id)
                return BadRequest();

            var result = await _storeService.UpdatePenaltyAsync(storeId, penaltyId, penalty);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{storeId}/mall/{mallId}")]
        public async Task<IActionResult> AddStoreToMall(int storeId, int mallId)
        {
            var result = await _storeService.AddStoreToMallAsync(storeId, mallId);
            if (!result)
                return BadRequest("Failed to add store to mall");

            return NoContent();
        }
    }
} 