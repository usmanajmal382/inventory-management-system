using inventory.application.DTOs;
using inventory.application.Services;
using Microsoft.AspNetCore.Mvc;

namespace inventory_management_system.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class StockAlertsController(IStockAlertService svc) : ControllerBase
    {
        [HttpGet] public async Task<ActionResult<IEnumerable<StockAlertDto>>> Get() => Ok(await svc.GetAllAlertsAsync());
        [HttpGet("unread")] public async Task<ActionResult<IEnumerable<StockAlertDto>>> GetUnread() => Ok(await svc.GetUnreadAlertsAsync());
        [HttpGet("product/{productId:int}")] public async Task<ActionResult<IEnumerable<StockAlertDto>>> ByProduct(int productId) => Ok(await svc.GetAlertsByProductIdAsync(productId));
        [HttpPatch("generate")] public async Task<IActionResult> Generate() { await svc.GenerateLowStockAlertsAsync(); return NoContent(); }
        [HttpPatch("{id:int}/read")] public async Task<IActionResult> Read(int id) { await svc.MarkAlertAsReadAsync(id); return NoContent(); }
        [HttpPatch("read-all")] public async Task<IActionResult> ReadAll() { await svc.MarkAllAlertsAsReadAsync(); return NoContent(); }
        [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) { await svc.DeleteAlertAsync(id); return NoContent(); }
    }
}
