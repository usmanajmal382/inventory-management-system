using inventory.application.DTOs;
using inventory.application.Interfaces;
using inventory.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.Services
{

    public class CategoryService(ICategoryRepository repo) : ICategoryService
    {
        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var c = await repo.GetByIdAsync(id);
            return c is null ? null : Map(c);
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync() =>
            (await repo.GetAllAsync()).Select(Map);

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
        {
            var category = new Category { Name = dto.Name, Description = dto.Description };
            await repo.AddAsync(category);
            return Map(category);
        }

        public async Task<CategoryDto> UpdateCategoryAsync(int id, CreateCategoryDto dto)
        {
            var c = await repo.GetByIdAsync(id) ?? throw new ArgumentException("Category not found");
            c.Name = dto.Name;
            c.Description = dto.Description;
            await repo.UpdateAsync(c);
            return Map(c);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            if (!await repo.ExistsAsync(id)) throw new ArgumentException("Category not found");
            await repo.DeleteAsync(id);
        }

        private static CategoryDto Map(Category c) => new(c.Id, c.Name, c.Description, c.Products?.Count ?? 0);
    }
}
