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
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _svc;

        public ProductsController(IProductService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        [RequirePermission("products.read")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get() =>
            Ok(await _svc.GetAllProductsAsync());

        [HttpGet("{id:int}")]
        [RequirePermission("products.read")]
        public async Task<ActionResult<ProductDto>> Get(int id) =>
            await _svc.GetProductByIdAsync(id) is { } p ? Ok(p) : NotFound();

        [HttpGet("sku/{sku}")]
        [RequirePermission("products.read")]
        public async Task<ActionResult<ProductDto>> GetBySku(string sku) =>
            await _svc.GetProductBySkuAsync(sku) is { } p ? Ok(p) : NotFound();

        [HttpGet("category/{categoryId:int}")]
        [RequirePermission("products.read")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> ByCategory(int categoryId) =>
            Ok(await _svc.GetProductsByCategoryAsync(categoryId));

        [HttpGet("low-stock")]
        [RequirePermission("products.read")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> LowStock() =>
            Ok(await _svc.GetLowStockProductsAsync());

        [HttpPost]
        [RequirePermission("products.create")]
        public async Task<ActionResult<ProductDto>> Post(CreateProductDto dto) =>
            CreatedAtAction(nameof(Get), new { id = (await _svc.CreateProductAsync(dto)).Id }, await _svc.CreateProductAsync(dto));

        [HttpPut("{id:int}")]
        [RequirePermission("products.update")]
        public async Task<ActionResult<ProductDto>> Put(int id, UpdateProductDto dto) =>
            Ok(await _svc.UpdateProductAsync(id, dto));

        [HttpDelete("{id:int}")]
        [RequirePermission("products.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _svc.DeleteProductAsync(id);
            return NoContent();
        }
    }
}