using AutoMapper;
using inventory.application.DTOs;
using inventory.application.Interfaces;
using inventory.application.Services;
using inventory.core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController(ISupplierService svc) : ControllerBase
    {
        [HttpGet] public async Task<ActionResult<IEnumerable<SupplierDto>>> Get() => Ok(await svc.GetAllSuppliersAsync());
        [HttpGet("active")] public async Task<ActionResult<IEnumerable<SupplierDto>>> GetActive() => Ok(await svc.GetActiveSuppliersAsync());
        [HttpGet("{id:int}")] public async Task<ActionResult<SupplierDto>> Get(int id) => await svc.GetSupplierByIdAsync(id) is { } s ? Ok(s) : NotFound();
        [HttpPost] public async Task<ActionResult<SupplierDto>> Post(CreateSupplierDto dto) => CreatedAtAction(nameof(Get), new { id = (await svc.CreateSupplierAsync(dto)).Id }, await svc.CreateSupplierAsync(dto));
        [HttpPut("{id:int}")] public async Task<ActionResult<SupplierDto>> Put(int id, CreateSupplierDto dto) => Ok(await svc.UpdateSupplierAsync(id, dto));
        [HttpPatch("{id:int}/deactivate")] public async Task<IActionResult> Deactivate(int id) { await svc.DeactivateSupplierAsync(id); return NoContent(); }
        [HttpPatch("{id:int}/activate")] public async Task<IActionResult> Activate(int id) { await svc.ActivateSupplierAsync(id); return NoContent(); }
        [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) { await svc.DeleteSupplierAsync(id); return NoContent(); }
    }
}
