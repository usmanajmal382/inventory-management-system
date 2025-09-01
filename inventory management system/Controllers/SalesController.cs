using inventory.application.Services;
using inventory_management_system;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _svc;

        public SalesController(ISalesService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        [RequirePermission("sales.read")]
        public async Task<ActionResult<IEnumerable<SaleDto>>> Get() =>
            Ok(await _svc.GetAllSalesAsync());

        [HttpGet("{id:int}")]
        [RequirePermission("sales.read")]
        public async Task<ActionResult<SaleDto>> Get(int id) =>
            await _svc.GetSaleByIdAsync(id) is { } sale ? Ok(sale) : NotFound();

        [HttpGet("date-range")]
        [RequirePermission("sales.read")]
        public async Task<ActionResult<IEnumerable<SaleDto>>> ByDateRange(DateTime from, DateTime to) =>
            Ok(await _svc.GetSalesByDateRangeAsync(from, to));

        [HttpGet("daily-report")]
        [RequirePermission("reports.read")]
        public async Task<ActionResult<DailySalesReportDto>> DailyReport(DateTime date) =>
            Ok(await _svc.GetDailySalesReportAsync(date));
    }
}