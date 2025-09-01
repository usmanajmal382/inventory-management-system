using inventory.application.DTOs;
using inventory.application.Services;
using inventory_management_system;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitService _svc;

        public UnitsController(IUnitService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        [RequirePermission("categories.read")]
        public async Task<ActionResult<IEnumerable<UnitDto>>> Get() =>
            Ok(await _svc.GetAllUnitsAsync());

        [HttpGet("active")]
        [RequirePermission("categories.read")]
        public async Task<ActionResult<IEnumerable<UnitDto>>> GetActive() =>
            Ok(await _svc.GetActiveUnitsAsync());

        [HttpGet("{id:int}")]
        [RequirePermission("categories.read")]
        public async Task<ActionResult<UnitDto>> Get(int id) =>
            await _svc.GetUnitByIdAsync(id) is { } unit ? Ok(unit) : NotFound();

        [HttpPost]
        [RequirePermission("categories.create")]
        public async Task<ActionResult<UnitDto>> Post(CreateUnitDto dto) =>
            CreatedAtAction(nameof(Get), new { id = (await _svc.CreateUnitAsync(dto)).Id }, await _svc.CreateUnitAsync(dto));

        [HttpPut("{id:int}")]
        [RequirePermission("categories.update")]
        public async Task<ActionResult<UnitDto>> Put(int id, UpdateUnitDto dto) =>
            Ok(await _svc.UpdateUnitAsync(id, dto));

        [HttpPatch("{id:int}/deactivate")]
        [RequirePermission("categories.update")]
        public async Task<IActionResult> Deactivate(int id)
        {
            await _svc.DeactivateUnitAsync(id);
            return NoContent();
        }

        [HttpPatch("{id:int}/activate")]
        [RequirePermission("categories.update")]
        public async Task<IActionResult> Activate(int id)
        {
            await _svc.ActivateUnitAsync(id);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [RequirePermission("categories.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _svc.DeleteUnitAsync(id);
            return NoContent();
        }
    }
}