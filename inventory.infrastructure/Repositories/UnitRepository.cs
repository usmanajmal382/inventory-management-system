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

    public class UnitRepository(AppDbContext ctx) : IUnitRepository
    {
        public async Task<Unit?> GetByIdAsync(int id) =>
            await ctx.Units.Include(u => u.Products).FirstOrDefaultAsync(u => u.Id == id);

        public async Task<IEnumerable<Unit>> GetAllAsync() =>
            await ctx.Units.Include(u => u.Products).OrderBy(u => u.Name).ToListAsync();

        public async Task<IEnumerable<Unit>> GetActiveAsync() =>
            await ctx.Units.Where(u => u.IsActive).OrderBy(u => u.Name).ToListAsync();

        public async Task<Unit> AddAsync(Unit unit)
        {
            ctx.Units.Add(unit);
            await ctx.SaveChangesAsync();
            return unit;
        }

        public async Task UpdateAsync(Unit unit)
        {
            ctx.Units.Update(unit);
            await ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var unit = await ctx.Units.FindAsync(id);
            if (unit != null)
            {
                ctx.Units.Remove(unit);
                await ctx.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id) =>
            await ctx.Units.AnyAsync(u => u.Id == id);
    }
}
