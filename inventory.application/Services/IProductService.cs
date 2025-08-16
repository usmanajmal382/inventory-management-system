using inventory.application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.Services
{
    public interface IProductService
    {
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<ProductDto?> GetProductBySkuAsync(string sku);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductDto>> GetLowStockProductsAsync();
        Task<ProductDto> CreateProductAsync(CreateProductDto dto);
        Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto dto);
        Task DeleteProductAsync(int id);
    }
}
