using inventory.application.DTOs;
using inventory.application.Services;
using inventory.core.Entities;
using inventory_management_system;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly IPurchaseOrderService _svc;

        public PurchaseOrdersController(IPurchaseOrderService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        [RequirePermission("purchase-orders.read")]
        public async Task<ActionResult<IEnumerable<PurchaseOrderDto>>> Get() =>
            Ok(await _svc.GetAllPurchaseOrdersAsync());

        [HttpGet("{id:int}")]
        [RequirePermission("purchase-orders.read")]
        public async Task<ActionResult<PurchaseOrderDto>> Get(int id) =>
            await _svc.GetPurchaseOrderByIdAsync(id) is { } po ? Ok(po) : NotFound();

        [HttpGet("supplier/{supplierId:int}")]
        [RequirePermission("purchase-orders.read")]
        public async Task<ActionResult<IEnumerable<PurchaseOrderDto>>> BySupplier(int supplierId) =>
            Ok(await _svc.GetPurchaseOrdersBySupplierAsync(supplierId));

        [HttpGet("status/{status}")]
        [RequirePermission("purchase-orders.read")]
        public async Task<ActionResult<IEnumerable<PurchaseOrderDto>>> ByStatus(PurchaseOrderStatus status) =>
            Ok(await _svc.GetPurchaseOrdersByStatusAsync(status));

        [HttpPost]
        [RequirePermission("purchase-orders.create")]
        public async Task<ActionResult<PurchaseOrderDto>> Post(CreatePurchaseOrderDto dto) =>
            CreatedAtAction(nameof(Get), new { id = (await _svc.CreatePurchaseOrderAsync(dto)).Id }, await _svc.CreatePurchaseOrderAsync(dto));

        [HttpPatch("{id:int}/status")]
        [RequirePermission("purchase-orders.update")]
        public async Task<ActionResult<PurchaseOrderDto>> PatchStatus(int id, [FromBody] PurchaseOrderStatus status) =>
            Ok(await _svc.UpdatePurchaseOrderStatusAsync(id, status));

        [HttpPatch("{id:int}/receive")]
        [RequirePermission("purchase-orders.update")]
        public async Task<ActionResult<PurchaseOrderDto>> Receive(int id) =>
            Ok(await _svc.ReceivePurchaseOrderAsync(id));

        [HttpDelete("{id:int}")]
        [RequirePermission("purchase-orders.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _svc.DeletePurchaseOrderAsync(id);
            return NoContent();
        }
    }
}