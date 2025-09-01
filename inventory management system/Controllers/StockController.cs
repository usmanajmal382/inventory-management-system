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
    public class StockController : ControllerBase
    {
        private readonly IStockService _svc;

        public StockController(IStockService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        [RequirePermission("stock.read")]
        public async Task<ActionResult<IEnumerable<StockTransactionDto>>> Get() =>
            Ok(await _svc.GetAllStockTransactionsAsync());

        [HttpGet("product/{productId:int}")]
        [RequirePermission("stock.read")]
        public async Task<ActionResult<IEnumerable<StockTransactionDto>>> ByProduct(int productId) =>
            Ok(await _svc.GetStockTransactionsByProductIdAsync(productId));

        [HttpPost]
        [RequirePermission("stock.create")]
        public async Task<ActionResult<StockTransactionDto>> Post(CreateStockTransactionDto dto) =>
            CreatedAtAction(nameof(ByProduct), new { productId = dto.ProductId }, await _svc.CreateStockTransactionAsync(dto));
    }
}