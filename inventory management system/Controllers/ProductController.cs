using AutoMapper;
using inventory.application.DTOs;
using inventory.application.Interfaces;
using inventory.application.Services;
using inventory.core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IProductService svc) : ControllerBase
    {
        [HttpGet] public async Task<ActionResult<IEnumerable<ProductDto>>> Get() => Ok(await svc.GetAllProductsAsync());
        [HttpGet("{id:int}")] public async Task<ActionResult<ProductDto>> Get(int id) => await svc.GetProductByIdAsync(id) is { } p ? Ok(p) : NotFound();
        [HttpGet("sku/{sku}")] public async Task<ActionResult<ProductDto>> GetBySku(string sku) => await svc.GetProductBySkuAsync(sku) is { } p ? Ok(p) : NotFound();
        [HttpGet("category/{categoryId:int}")] public async Task<ActionResult<IEnumerable<ProductDto>>> ByCategory(int categoryId) => Ok(await svc.GetProductsByCategoryAsync(categoryId));
        [HttpGet("low-stock")] public async Task<ActionResult<IEnumerable<ProductDto>>> LowStock() => Ok(await svc.GetLowStockProductsAsync());
        [HttpPost] public async Task<ActionResult<ProductDto>> Post(CreateProductDto dto) => CreatedAtAction(nameof(Get), new { id = (await svc.CreateProductAsync(dto)).Id }, await svc.CreateProductAsync(dto));
        [HttpPut("{id:int}")] public async Task<ActionResult<ProductDto>> Put(int id, UpdateProductDto dto) => Ok(await svc.UpdateProductAsync(id, dto));
        [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) { await svc.DeleteProductAsync(id); return NoContent(); }
    }
}