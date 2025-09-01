using inventory.application.DTOs;
using inventory.application.Services;
using inventory_management_system;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _svc;

        public SuppliersController(ISupplierService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        [RequirePermission("suppliers.read")]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> Get() =>
            Ok(await _svc.GetAllSuppliersAsync());

        [HttpGet("active")]
        [RequirePermission("suppliers.read")]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetActive() =>
            Ok(await _svc.GetActiveSuppliersAsync());

        [HttpGet("{id:int}")]
        [RequirePermission("suppliers.read")]
        public async Task<ActionResult<SupplierDto>> Get(int id) =>
            await _svc.GetSupplierByIdAsync(id) is { } s ? Ok(s) : NotFound();

        [HttpPost]
        [RequirePermission("suppliers.create")]
        public async Task<ActionResult<SupplierDto>> Post(CreateSupplierDto dto) =>
            CreatedAtAction(nameof(Get), new { id = (await _svc.CreateSupplierAsync(dto)).Id }, await _svc.CreateSupplierAsync(dto));

        [HttpPut("{id:int}")]
        [RequirePermission("suppliers.update")]
        public async Task<ActionResult<SupplierDto>> Put(int id, CreateSupplierDto dto) =>
            Ok(await _svc.UpdateSupplierAsync(id, dto));

        [HttpPatch("{id:int}/deactivate")]
        [RequirePermission("suppliers.update")]
        public async Task<IActionResult> Deactivate(int id)
        {
            await _svc.DeactivateSupplierAsync(id);
            return NoContent();
        }

        [HttpPatch("{id:int}/activate")]
        [RequirePermission("suppliers.update")]
        public async Task<IActionResult> Activate(int id)
        {
            await _svc.ActivateSupplierAsync(id);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [RequirePermission("suppliers.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _svc.DeleteSupplierAsync(id);
            return NoContent();
        }
    }
}