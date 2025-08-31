using inventory.application.Interfaces;
using inventory.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.Services
{

    public class SalesService(
        ISaleRepository saleRepo,
        IOrderRepository orderRepo) : ISalesService
    {
        public async Task<SaleDto?> GetSaleByIdAsync(int id)
        {
            var sale = await saleRepo.GetByIdAsync(id);
            return sale is not null ? Map(sale) : null;
        }

        public async Task<IEnumerable<SaleDto>> GetAllSalesAsync() =>
            (await saleRepo.GetAllAsync()).Select(Map);

        public async Task<IEnumerable<SaleDto>> GetSalesByDateRangeAsync(DateTime from, DateTime to) =>
            (await saleRepo.GetByDateRangeAsync(from, to)).Select(Map);

        public async Task<DailySalesReportDto> GetDailySalesReportAsync(DateTime date)
        {
            var from = date.Date;
            var to = from.AddDays(1).AddTicks(-1);
            var sales = await saleRepo.GetByDateRangeAsync(from, to);

            return new DailySalesReportDto(
                Date: date,
                TotalSales: sales.Count(),
                TotalRevenue: sales.Sum(s => s.TotalAmount),
                TotalTax: sales.Sum(s => s.TaxAmount),
                TotalDiscount: sales.Sum(s => s.DiscountAmount)
            );
        }

        public async Task<SaleDto> CreateSaleFromOrderAsync(int orderId)
        {
            var order = await orderRepo.GetByIdAsync(orderId)
                ?? throw new ArgumentException("Order not found");

            if (order.Status != OrderStatus.Delivered)
                throw new ArgumentException("Only delivered orders can be converted to sales");

            var sale = new Sale
            {
                SaleNumber = $"SL{DateTime.UtcNow:yyyyMMddHHmmss}",
                SaleDate = DateTime.UtcNow,
                CustomerName = order.CustomerName,
                TotalAmount = order.TotalAmount,
                TaxAmount = order.TaxAmount,
                DiscountAmount = order.DiscountAmount,
                OrderId = order.Id,
                Items = order.Items.Select(i => new SaleItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Unit = i.Unit
                }).ToList()
            };

            sale = await saleRepo.AddAsync(sale);
            return Map(sale);
        }

        private static SaleDto Map(Sale s) => new(
            s.Id,
            s.SaleNumber,
            s.SaleDate,
            s.CustomerName,
            s.TotalAmount,
            s.TaxAmount,
            s.DiscountAmount,
            s.Items.Select(i => new SaleItemDto(
                i.Id,
                i.ProductId,
                i.Product?.Name,
                i.Product?.SKU,
                i.Quantity,
                i.UnitPrice,
                i.Quantity * i.UnitPrice,
                i.Unit
            )).ToList());
    }
}
