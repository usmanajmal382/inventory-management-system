using AutoMapper;
using inventory.application.DTOs;
using inventory.application.Interfaces;
using inventory.core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<CategoryDto>>(categories));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            return category == null ? NotFound() : Ok(_mapper.Map<CategoryDto>(category));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            await _categoryRepo.AddAsync(category);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, _mapper.Map<CategoryDto>(category));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryDto dto)
        {
            var existing = await _categoryRepo.GetByIdAsync(id);
            if (existing == null) return NotFound();

            _mapper.Map(dto, existing);
            _categoryRepo.Update(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null) return NotFound();

            _categoryRepo.Delete(category);
            return NoContent();
        }
    }
}
