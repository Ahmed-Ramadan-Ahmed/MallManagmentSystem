using Microsoft.AspNetCore.Mvc;
using MallManagmentSystem.Interfaces;
using MallManagmentSystem.Models;

namespace MallManagmentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MallController : ControllerBase
    {
        private readonly IMallService _mallService;

        public MallController(IMallService mallService)
        {
            _mallService = mallService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Mall>>> GetAllMalls()
        {
            var malls = await _mallService.GetAllMallsAsync();
            return Ok(malls);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Mall>> GetMallById(int id)
        {
            var mall = await _mallService.GetMallByIdAsync(id);
            if (mall == null)
                return NotFound();

            return Ok(mall);
        }

        [HttpPost]
        public async Task<ActionResult<Mall>> CreateMall(Mall mall)
        {
            var result = await _mallService.CreateMallAsync(mall);
            if (!result)
                return BadRequest("Failed to create mall");

            return CreatedAtAction(nameof(GetMallById), new { id = mall.Id }, mall);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMall(int id, Mall mall)
        {
            if (id != mall.Id)
                return BadRequest();

            var result = await _mallService.UpdateMallAsync(id, mall);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMall(int id)
        {
            var result = await _mallService.DeleteMallAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/statistics")]
        public async Task<IActionResult> UpdateMallStatistics(int id)
        {
            var result = await _mallService.UpdateMallStatisticsAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
