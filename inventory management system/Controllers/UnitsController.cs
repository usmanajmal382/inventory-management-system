using inventory.application.DTOs;
using inventory.application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inventory_management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitsController(IUnitService svc) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UnitDto>>> Get() =>
            Ok(await svc.GetAllUnitsAsync());

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<UnitDto>>> GetActive() =>
            Ok(await svc.GetActiveUnitsAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UnitDto>> Get(int id) =>
            await svc.GetUnitByIdAsync(id) is { } unit ? Ok(unit) : NotFound();

        [HttpPost]
        public async Task<ActionResult<UnitDto>> Post(CreateUnitDto dto) =>
            CreatedAtAction(nameof(Get), new { id = (await svc.CreateUnitAsync(dto)).Id }, await svc.CreateUnitAsync(dto));

        [HttpPut("{id:int}")]
        public async Task<ActionResult<UnitDto>> Put(int id, UpdateUnitDto dto) =>
            Ok(await svc.UpdateUnitAsync(id, dto));

        [HttpPatch("{id:int}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            await svc.DeactivateUnitAsync(id);
            return NoContent();
        }

        [HttpPatch("{id:int}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            await svc.ActivateUnitAsync(id);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await svc.DeleteUnitAsync(id);
            return NoContent();
        }
    }
}
