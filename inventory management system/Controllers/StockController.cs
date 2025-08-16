using inventory.application.DTOs;
using inventory.application.Services;
using Microsoft.AspNetCore.Mvc;

namespace inventory_management_system.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController(IStockService svc) : ControllerBase
    {
        [HttpGet] public async Task<ActionResult<IEnumerable<StockTransactionDto>>> Get() => Ok(await svc.GetAllStockTransactionsAsync());
        [HttpGet("product/{productId:int}")] public async Task<ActionResult<IEnumerable<StockTransactionDto>>> ByProduct(int productId) => Ok(await svc.GetStockTransactionsByProductIdAsync(productId));
        [HttpPost] public async Task<ActionResult<StockTransactionDto>> Post(CreateStockTransactionDto dto) => CreatedAtAction(nameof(ByProduct), new { productId = dto.ProductId }, await svc.CreateStockTransactionAsync(dto));
    }
}
