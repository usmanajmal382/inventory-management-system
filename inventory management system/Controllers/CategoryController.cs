using AutoMapper;
using inventory.application.DTOs;
using inventory.application.Interfaces;
using inventory.application.Services;
using inventory.core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController(ICategoryService svc) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> Get() => Ok(await svc.GetAllCategoriesAsync());
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDto>> Get(int id) => await svc.GetCategoryByIdAsync(id) is { } c ? Ok(c) : NotFound();
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Post(CreateCategoryDto dto) => CreatedAtAction(nameof(Get), new { id = (await svc.CreateCategoryAsync(dto)).Id }, await svc.CreateCategoryAsync(dto));
        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoryDto>> Put(int id, CreateCategoryDto dto) => Ok(await svc.UpdateCategoryAsync(id, dto));
        [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) { await svc.DeleteCategoryAsync(id); return NoContent(); }
    }
}
