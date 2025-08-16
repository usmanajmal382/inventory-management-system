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
    public class PurchaseOrderRepository(AppDbContext ctx) : IPurchaseOrderRepository
    {
        public async Task<PurchaseOrder?> GetByIdAsync(int id) =>
            await ctx.PurchaseOrders
                .Include(po => po.Supplier)
                .Include(po => po.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(po => po.Id == id);

        public async Task<PurchaseOrder?> GetByOrderNumberAsync(string orderNumber) =>
            await ctx.PurchaseOrders
                .Include(po => po.Supplier)
                .Include(po => po.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(po => po.OrderNumber == orderNumber);

        public async Task<IEnumerable<PurchaseOrder>> GetAllAsync() =>
            await ctx.PurchaseOrders
                .Include(po => po.Supplier)
                .Include(po => po.Items)
                .OrderByDescending(po => po.OrderDate)
                .ToListAsync();

        public async Task<IEnumerable<PurchaseOrder>> GetBySupplierAsync(int supplierId) =>
            await ctx.PurchaseOrders
                .Include(po => po.Supplier)
                .Include(po => po.Items)
                .Where(po => po.SupplierId == supplierId)
                .OrderByDescending(po => po.OrderDate)
                .ToListAsync();

        public async Task<IEnumerable<PurchaseOrder>> GetByStatusAsync(PurchaseOrderStatus status) =>
            await ctx.PurchaseOrders
                .Include(po => po.Supplier)
                .Include(po => po.Items)
                .Where(po => po.Status == status)
                .OrderByDescending(po => po.OrderDate)
                .ToListAsync();

        public async Task<PurchaseOrder> AddAsync(PurchaseOrder po)
        {
            ctx.PurchaseOrders.Add(po);
            await ctx.SaveChangesAsync();
            return po;
        }

        public async Task UpdateAsync(PurchaseOrder po)
        {
            ctx.PurchaseOrders.Update(po);
            await ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var po = await ctx.PurchaseOrders.FindAsync(id);
            if (po is not null) { ctx.PurchaseOrders.Remove(po); await ctx.SaveChangesAsync(); }
        }

        public async Task<bool> ExistsAsync(int id) =>
            await ctx.PurchaseOrders.AnyAsync(po => po.Id == id);

        public async Task<string> GenerateOrderNumberAsync()
        {
            var year = DateTime.UtcNow.Year;
            var prefix = $"PO{year}";
            var last = await ctx.PurchaseOrders
                .Where(po => po.OrderNumber.StartsWith(prefix))
                .OrderByDescending(po => po.OrderNumber)
                .FirstOrDefaultAsync();
            var next = 1;
            if (last is not null)
            {
                var numPart = last.OrderNumber[prefix.Length..];
                if (int.TryParse(numPart, out var lastNum))
                    next = lastNum + 1;
            }
            return $"{prefix}{next:D4}";
        }
    }
}
