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
    public class ProductService(
      IProductRepository prodRepo,
      ICategoryRepository catRepo) : IProductService
    {
        public async Task<ProductDto?> GetProductByIdAsync(int id) =>
            Map(await prodRepo.GetByIdAsync(id));

        public async Task<ProductDto?> GetProductBySkuAsync(string sku) =>
            Map(await prodRepo.GetBySkuAsync(sku));

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync() =>
            (await prodRepo.GetAllAsync()).Select(Map);

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId) =>
            (await prodRepo.GetByCategoryAsync(categoryId)).Select(Map);

        public async Task<IEnumerable<ProductDto>> GetLowStockProductsAsync() =>
            (await prodRepo.GetLowStockProductsAsync()).Select(Map);

        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
        {
            if (!await catRepo.ExistsAsync(dto.CategoryId))
                throw new ArgumentException("Category does not exist");
            if (await prodRepo.SkuExistsAsync(dto.SKU))
                throw new ArgumentException("SKU already exists");

            var p = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                SKU = dto.SKU,
                Price = dto.Price,
                Quantity = dto.Quantity,
                MinStockLevel = dto.MinStockLevel,
                CategoryId = dto.CategoryId
            };
            await prodRepo.AddAsync(p);
            return Map(p)!;
        }

        public async Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            var p = await prodRepo.GetByIdAsync(id) ?? throw new ArgumentException("Product not found");
            if (!await catRepo.ExistsAsync(dto.CategoryId))
                throw new ArgumentException("Category does not exist");

            p.Name = dto.Name;
            p.Description = dto.Description;
            p.Price = dto.Price;
            p.MinStockLevel = dto.MinStockLevel;
            p.CategoryId = dto.CategoryId;
            p.UpdatedAt = DateTime.UtcNow;
            await prodRepo.UpdateAsync(p);
            return Map(p)!;
        }

        public async Task DeleteProductAsync(int id)
        {
            if (!await prodRepo.ExistsAsync(id)) throw new ArgumentException("Product not found");
            await prodRepo.DeleteAsync(id);
        }

        private static ProductDto? Map(Product? p) => p is null ? null :
            new(p.Id, p.Name, p.Description, p.SKU, p.Price, p.Quantity, p.MinStockLevel,
                p.CategoryId, p.Category?.Name, p.CreatedAt, p.UpdatedAt, p.Quantity <= p.MinStockLevel);
    }
}
