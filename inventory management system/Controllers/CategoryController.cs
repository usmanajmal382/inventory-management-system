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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _svc;

        public CategoriesController(ICategoryService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        [RequirePermission("categories.read")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> Get() =>
            Ok(await _svc.GetAllCategoriesAsync());

        [HttpGet("{id:int}")]
        [RequirePermission("categories.read")]
        public async Task<ActionResult<CategoryDto>> Get(int id) =>
            await _svc.GetCategoryByIdAsync(id) is { } c ? Ok(c) : NotFound();

        [HttpPost]
        [RequirePermission("categories.create")]
        public async Task<ActionResult<CategoryDto>> Post(CreateCategoryDto dto) =>
            CreatedAtAction(nameof(Get), new { id = (await _svc.CreateCategoryAsync(dto)).Id }, await _svc.CreateCategoryAsync(dto));

        [HttpPut("{id:int}")]
        [RequirePermission("categories.update")]
        public async Task<ActionResult<CategoryDto>> Put(int id, CreateCategoryDto dto) =>
            Ok(await _svc.UpdateCategoryAsync(id, dto));

        [HttpDelete("{id:int}")]
        [RequirePermission("categories.delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _svc.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}