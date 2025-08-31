using inventory.application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inventory_management_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController(ISalesService svc) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleDto>>> Get() =>
            Ok(await svc.GetAllSalesAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SaleDto>> Get(int id) =>
            await svc.GetSaleByIdAsync(id) is { } sale ? Ok(sale) : NotFound();

        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<SaleDto>>> ByDateRange(DateTime from, DateTime to) =>
            Ok(await svc.GetSalesByDateRangeAsync(from, to));

        [HttpGet("daily-report")]
        public async Task<ActionResult<DailySalesReportDto>> DailyReport(DateTime date) =>
            Ok(await svc.GetDailySalesReportAsync(date));
    }
}
