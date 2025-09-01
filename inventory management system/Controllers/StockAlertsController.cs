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
    public class StockAlertsController : ControllerBase
    {
        private readonly IStockAlertService _svc;

        public StockAlertsController(IStockAlertService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        [RequirePermission("alerts.read")]
        public async Task<ActionResult<IEnumerable<StockAlertDto>>> Get() =>
            Ok(await _svc.GetAllAlertsAsync());

        [HttpGet("unread")]
        [RequirePermission("alerts.read")]
        public async Task<ActionResult<IEnumerable<StockAlertDto>>> GetUnread() =>
            Ok(await _svc.GetUnreadAlertsAsync());

        [HttpGet("product/{productId:int}")]
        [RequirePermission("alerts.read")]
        public async Task<ActionResult<IEnumerable<StockAlertDto>>> ByProduct(int productId) =>
            Ok(await _svc.GetAlertsByProductIdAsync(productId));

        [HttpPatch("generate")]
        [RequirePermission("alerts.update")]
        public async Task<IActionResult> Generate()
        {
            await _svc.GenerateLowStockAlertsAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}/read")]
        [RequirePermission("alerts.update")]
        public async Task<IActionResult> Read(int id)
        {
            await _svc.MarkAlertAsReadAsync(id);
            return NoContent();
        }

        [HttpPatch("read-all")]
        [RequirePermission("alerts.update")]
        public async Task<IActionResult> ReadAll()
        {
            await _svc.MarkAllAlertsAsReadAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [RequirePermission("alerts.update")]
        public async Task<IActionResult> Delete(int id)
        {
            await _svc.DeleteAlertAsync(id);
            return NoContent();
        }
    }
}