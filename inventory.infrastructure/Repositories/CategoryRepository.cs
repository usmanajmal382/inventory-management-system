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
    public class CategoryRepository(AppDbContext ctx) : ICategoryRepository
    {
        public async Task<Category?> GetByIdAsync(int id) =>
            await ctx.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);

        public async Task<IEnumerable<Category>> GetAllAsync() =>
            await ctx.Categories.Include(c => c.Products).OrderBy(c => c.Name).ToListAsync();

        public async Task<Category> AddAsync(Category category)
        {
            ctx.Categories.Add(category);
            await ctx.SaveChangesAsync();
            return category;
        }

        public async Task UpdateAsync(Category category)
        {
            ctx.Categories.Update(category);
            await ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var c = await ctx.Categories.FindAsync(id);
            if (c is not null) { ctx.Categories.Remove(c); await ctx.SaveChangesAsync(); }
        }

        public async Task<bool> ExistsAsync(int id) =>
            await ctx.Categories.AnyAsync(c => c.Id == id);
    }
}
