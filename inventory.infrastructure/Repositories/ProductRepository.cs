using inventory.application.Interfaces;
using inventory.core.Entities;
using Microsoft.EntityFrameworkCore;
using MyApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.infrastructure.Repositories
{
    public class ProductRepository(AppDbContext ctx) : IProductRepository
    {
        public async Task<Product?> GetByIdAsync(int id) =>
            await ctx.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Product?> GetBySkuAsync(string sku) =>
            await ctx.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.SKU == sku);

        public async Task<IEnumerable<Product>> GetAllAsync() =>
            await ctx.Products.Include(p => p.Category).OrderBy(p => p.Name).ToListAsync();

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId) =>
            await ctx.Products.Include(p => p.Category).Where(p => p.CategoryId == categoryId).OrderBy(p => p.Name).ToListAsync();

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync() =>
            await ctx.Products.Include(p => p.Category).Where(p => p.Quantity <= p.MinStockLevel).OrderBy(p => p.Quantity).ToListAsync();

        public async Task<Product> AddAsync(Product product)
        {
            ctx.Products.Add(product);
            await ctx.SaveChangesAsync();
            return product;
        }

        public async Task UpdateAsync(Product product)
        {
            ctx.Products.Update(product);
            await ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var p = await ctx.Products.FindAsync(id);
            if (p is not null) { ctx.Products.Remove(p); await ctx.SaveChangesAsync(); }
        }

        public async Task<bool> ExistsAsync(int id) =>
            await ctx.Products.AnyAsync(p => p.Id == id);

        public async Task<bool> SkuExistsAsync(string sku, int? excludeId = null)
        {
            var q = ctx.Products.Where(p => p.SKU == sku);
            if (excludeId.HasValue) q = q.Where(p => p.Id != excludeId.Value);
            return await q.AnyAsync();
        }


    }
}
