using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using inventory.application.DTOs;
using inventory.application.Interfaces;
using inventory.core.Entities;

namespace MyApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productRepo.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            return product == null ? NotFound() : Ok(_mapper.Map<ProductDto>(product));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            await _productRepo.AddAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, _mapper.Map<ProductDto>(product));
        }

        [HttpPut("{id}")]
        [Authorize]

        public async Task<IActionResult> Update(int id, ProductDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch");

            var existing = await _productRepo.GetByIdAsync(id);
            if (existing == null) return NotFound();

            _mapper.Map(dto, existing);
            _productRepo.Update(existing);
            // If your repository doesn’t call SaveChanges inside Update(), add:
            // await _context.SaveChangesAsync();
            return NoContent();   // 204
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return NotFound();

            _productRepo.Delete(product);
            // If your repository doesn’t call SaveChanges inside Delete(), add:
            // await _context.SaveChangesAsync();
            return NoContent();   // 204
        }
    }
}