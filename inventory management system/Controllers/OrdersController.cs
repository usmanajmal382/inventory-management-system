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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _svc;

        public OrdersController(IOrderService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        [RequirePermission("orders.read")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> Get() =>
            Ok(await _svc.GetAllOrdersAsync());

        [HttpGet("{id:int}")]
        [RequirePermission("orders.read")]
        public async Task<ActionResult<OrderDto>> Get(int id) =>
            await _svc.GetOrderByIdAsync(id) is { } order ? Ok(order) : NotFound();

        [HttpGet("status/{status}")]
        [RequirePermission("orders.read")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> ByStatus(OrderStatus status) =>
            Ok(await _svc.GetOrdersByStatusAsync(status));

        [HttpGet("customer/{customerName}")]
        [RequirePermission("orders.read")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> ByCustomer(string customerName) =>
            Ok(await _svc.GetOrdersByCustomerAsync(customerName));

        [HttpPost]
        [RequirePermission("orders.create")]
        public async Task<ActionResult<OrderDto>> Post(CreateOrderDto dto) =>
            CreatedAtAction(nameof(Get), new { id = (await _svc.CreateOrderAsync(dto)).Id }, await _svc.CreateOrderAsync(dto));

        [HttpPatch("{id:int}/confirm")]
        [RequirePermission("orders.update")]
        public async Task<ActionResult<OrderDto>> Confirm(int id) =>
            Ok(await _svc.ConfirmOrderAsync(id));

        [HttpPatch("{id:int}/ship")]
        [RequirePermission("orders.update")]
        public async Task<ActionResult<OrderDto>> Ship(int id) =>
            Ok(await _svc.ShipOrderAsync(id));

        [HttpPatch("{id:int}/deliver")]
        [RequirePermission("orders.update")]
        public async Task<ActionResult<OrderDto>> Deliver(int id) =>
            Ok(await _svc.DeliverOrderAsync(id));

        [HttpPatch("{id:int}/cancel")]
        [RequirePermission("orders.update")]
        public async Task<ActionResult<OrderDto>> Cancel(int id) =>
            Ok(await _svc.CancelOrderAsync(id));

        [HttpDelete("{id:int}")]
        [RequirePermission("orders.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _svc.DeleteOrderAsync(id);
            return NoContent();
        }
    }
}