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
    public class OrderRepository(AppDbContext ctx) : IOrderRepository
    {
        public async Task<Order?> GetByIdAsync(int id) =>
            await ctx.Orders
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

        public async Task<Order?> GetByOrderNumberAsync(string orderNumber) =>
            await ctx.Orders
                .Include(o => o.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);

        public async Task<IEnumerable<Order>> GetAllAsync() =>
            await ctx.Orders
                .Include(o => o.Items)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

        public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status) =>
            await ctx.Orders
                .Include(o => o.Items)
                .Where(o => o.Status == status)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

        public async Task<IEnumerable<Order>> GetByCustomerAsync(string customerName) =>
            await ctx.Orders
                .Include(o => o.Items)
                .Where(o => o.CustomerName != null &&
                            o.CustomerName.Contains(customerName))
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

        public async Task<Order> AddAsync(Order order)
        {
            ctx.Orders.Add(order);
            await ctx.SaveChangesAsync();
            return order;
        }

        public async Task UpdateAsync(Order order)
        {
            ctx.Orders.Update(order);
            await ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var order = await ctx.Orders.FindAsync(id);
            if (order != null)
            {
                ctx.Orders.Remove(order);
                await ctx.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id) =>
            await ctx.Orders.AnyAsync(o => o.Id == id);

        public async Task<string> GenerateOrderNumberAsync()
        {
            var year = DateTime.UtcNow.Year;
            var prefix = $"SO{year}";
            var last = await ctx.Orders
                .Where(o => o.OrderNumber.StartsWith(prefix))
                .OrderByDescending(o => o.OrderNumber)
                .FirstOrDefaultAsync();
            var next = 1;
            if (last != null)
            {
                var numPart = last.OrderNumber[prefix.Length..];
                if (int.TryParse(numPart, out var lastNum))
                    next = lastNum + 1;
            }
            return $"{prefix}{next:D4}";
        }
    }
}
