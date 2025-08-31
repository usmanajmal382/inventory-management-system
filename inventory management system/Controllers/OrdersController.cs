using inventory.application.DTOs;
using inventory.application.Services;
using inventory.core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inventory_management_system.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController(IOrderService svc) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> Get() =>
            Ok(await svc.GetAllOrdersAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> Get(int id) =>
            await svc.GetOrderByIdAsync(id) is { } order ? Ok(order) : NotFound();

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> ByStatus(OrderStatus status) =>
            Ok(await svc.GetOrdersByStatusAsync(status));

        [HttpGet("customer/{customerName}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> ByCustomer(string customerName) =>
            Ok(await svc.GetOrdersByCustomerAsync(customerName));

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Post(CreateOrderDto dto) =>
            CreatedAtAction(nameof(Get), new { id = (await svc.CreateOrderAsync(dto)).Id }, await svc.CreateOrderAsync(dto));

        [HttpPatch("{id:int}/confirm")]
        public async Task<ActionResult<OrderDto>> Confirm(int id) =>
            Ok(await svc.ConfirmOrderAsync(id));

        [HttpPatch("{id:int}/ship")]
        public async Task<ActionResult<OrderDto>> Ship(int id) =>
            Ok(await svc.ShipOrderAsync(id));

        [HttpPatch("{id:int}/deliver")]
        public async Task<ActionResult<OrderDto>> Deliver(int id) =>
            Ok(await svc.DeliverOrderAsync(id));

        [HttpPatch("{id:int}/cancel")]
        public async Task<ActionResult<OrderDto>> Cancel(int id) =>
            Ok(await svc.CancelOrderAsync(id));

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await svc.DeleteOrderAsync(id);
            return NoContent();
        }
    }
}
