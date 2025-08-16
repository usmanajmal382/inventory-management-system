using inventory.application.DTOs;
using inventory.application.Services;
using inventory.core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inventory_management_system.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseOrdersController(IPurchaseOrderService svc) : ControllerBase
    {
        [HttpGet] public async Task<ActionResult<IEnumerable<PurchaseOrderDto>>> Get() => Ok(await svc.GetAllPurchaseOrdersAsync());
        [HttpGet("{id:int}")] public async Task<ActionResult<PurchaseOrderDto>> Get(int id) => await svc.GetPurchaseOrderByIdAsync(id) is { } po ? Ok(po) : NotFound();
        [HttpGet("supplier/{supplierId:int}")] public async Task<ActionResult<IEnumerable<PurchaseOrderDto>>> BySupplier(int supplierId) => Ok(await svc.GetPurchaseOrdersBySupplierAsync(supplierId));
        [HttpGet("status/{status}")] public async Task<ActionResult<IEnumerable<PurchaseOrderDto>>> ByStatus(PurchaseOrderStatus status) => Ok(await svc.GetPurchaseOrdersByStatusAsync(status));
        [HttpPost] public async Task<ActionResult<PurchaseOrderDto>> Post(CreatePurchaseOrderDto dto) => CreatedAtAction(nameof(Get), new { id = (await svc.CreatePurchaseOrderAsync(dto)).Id }, await svc.CreatePurchaseOrderAsync(dto));
        [HttpPatch("{id:int}/status")] public async Task<ActionResult<PurchaseOrderDto>> PatchStatus(int id, [FromBody] PurchaseOrderStatus status) => Ok(await svc.UpdatePurchaseOrderStatusAsync(id, status));
        [HttpPatch("{id:int}/receive")] public async Task<ActionResult<PurchaseOrderDto>> Receive(int id) => Ok(await svc.ReceivePurchaseOrderAsync(id));
        [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) { await svc.DeletePurchaseOrderAsync(id); return NoContent(); }
    }
}
