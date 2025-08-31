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

    public class OrderService(
        IOrderRepository orderRepo,
        IProductRepository prodRepo,
        IStockTransactionRepository stockRepo) : IOrderService
    {
        public async Task<OrderDto?> GetOrderByIdAsync(int id) =>
            Map(await orderRepo.GetByIdAsync(id));

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync() =>
            (await orderRepo.GetAllAsync())
                .Select(Map)
                .Where(dto => dto is not null)!
                .Cast<OrderDto>();

        public async Task<IEnumerable<OrderDto>> GetOrdersByStatusAsync(OrderStatus status) =>
            (await orderRepo.GetByStatusAsync(status))
                .Select(Map)
                .Where(dto => dto is not null)!
                .Cast<OrderDto>();

        public async Task<IEnumerable<OrderDto>> GetOrdersByCustomerAsync(string customerName) =>
            (await orderRepo.GetByCustomerAsync(customerName))
                .Select(Map)
                .Where(dto => dto is not null)!
                .Cast<OrderDto>();

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto)
        {
            foreach (var item in dto.Items)
            {
                if (!await prodRepo.ExistsAsync(item.ProductId))
                    throw new ArgumentException($"Product {item.ProductId} does not exist");

                var product = await prodRepo.GetByIdAsync(item.ProductId);
                if (product != null && product.Quantity < item.Quantity)
                    throw new ArgumentException($"Insufficient stock for product {product.Name}");
            }

            var subtotal = dto.Items.Sum(i => i.Quantity * i.UnitPrice - i.Discount);
            var total = subtotal + dto.TaxAmount - dto.DiscountAmount;

            var order = new Order
            {
                OrderNumber = await orderRepo.GenerateOrderNumberAsync(),
                OrderDate = DateTime.UtcNow,
                DeliveryDate = dto.DeliveryDate,
                CustomerName = dto.CustomerName,
                ShippingAddress = dto.ShippingAddress,
                Phone = dto.Phone,
                Email = dto.Email,
                Status = OrderStatus.Pending,
                TotalAmount = total,
                TaxAmount = dto.TaxAmount,
                DiscountAmount = dto.DiscountAmount,
                Notes = dto.Notes,
                Items = dto.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Discount = i.Discount,
                    Unit = i.Unit
                }).ToList()
            };

            order = await orderRepo.AddAsync(order);
            var mappedOrder = Map(order);
            if (mappedOrder is null)
                throw new InvalidOperationException("Failed to map order to OrderDto.");
            return mappedOrder;
        }

        public async Task<OrderDto> UpdateOrderStatusAsync(int id, OrderStatus status)
        {
            var order = await orderRepo.GetByIdAsync(id) ?? throw new ArgumentException("Order not found");
            order.Status = status;
            await orderRepo.UpdateAsync(order);
            return Map(order);
        }

        public async Task<OrderDto> ConfirmOrderAsync(int id)
        {
            var order = await orderRepo.GetByIdAsync(id) ?? throw new ArgumentException("Order not found");

            if (order.Status != OrderStatus.Pending)
                throw new ArgumentException("Only pending orders can be confirmed");

            foreach (var item in order.Items)
            {
                var tx = new StockTransaction
                {
                    ProductId = item.ProductId,
                    Quantity = -item.Quantity,
                    Type = TransactionType.Sale,
                    Notes = $"Order: {order.OrderNumber}",
                    ReferenceNumber = order.OrderNumber,
                    CreatedAt = DateTime.UtcNow
                };
                await stockRepo.AddAsync(tx);

                var product = await prodRepo.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Quantity -= item.Quantity;
                    await prodRepo.UpdateAsync(product);
                }
            }

            order.Status = OrderStatus.Confirmed;
            await orderRepo.UpdateAsync(order);
            return Map(order);
        }

        public async Task<OrderDto> ShipOrderAsync(int id)
        {
            var order = await orderRepo.GetByIdAsync(id) ?? throw new ArgumentException("Order not found");
            if (order.Status != OrderStatus.Confirmed)
                throw new ArgumentException("Only confirmed orders can be shipped");

            order.Status = OrderStatus.Shipped;
            order.DeliveryDate = DateTime.UtcNow;
            await orderRepo.UpdateAsync(order);
            return Map(order);
        }

        public async Task<OrderDto> DeliverOrderAsync(int id)
        {
            var order = await orderRepo.GetByIdAsync(id) ?? throw new ArgumentException("Order not found");
            if (order.Status != OrderStatus.Shipped)
                throw new ArgumentException("Only shipped orders can be delivered");

            order.Status = OrderStatus.Delivered;
            await orderRepo.UpdateAsync(order);
            return Map(order);
        }

        public async Task<OrderDto> CancelOrderAsync(int id)
        {
            var order = await orderRepo.GetByIdAsync(id) ?? throw new ArgumentException("Order not found");
            if (order.Status is OrderStatus.Delivered or OrderStatus.Cancelled)
                throw new ArgumentException("Cannot cancel delivered or already cancelled order");

            if (order.Status == OrderStatus.Confirmed)
            {
                foreach (var item in order.Items)
                {
                    var tx = new StockTransaction
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Type = TransactionType.Return,
                        Notes = $"Order cancelled: {order.OrderNumber}",
                        ReferenceNumber = order.OrderNumber,
                        CreatedAt = DateTime.UtcNow
                    };
                    await stockRepo.AddAsync(tx);

                    var product = await prodRepo.GetByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        product.Quantity += item.Quantity;
                        await prodRepo.UpdateAsync(product);
                    }
                }
            }

            order.Status = OrderStatus.Cancelled;
            await orderRepo.UpdateAsync(order);
            return Map(order);
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await orderRepo.GetByIdAsync(id) ?? throw new ArgumentException("Order not found");
            if (order.Status is OrderStatus.Confirmed or OrderStatus.Shipped or OrderStatus.Delivered)
                throw new ArgumentException("Cannot delete confirmed/shipped/delivered orders");

            await orderRepo.DeleteAsync(id);
        }

        private static OrderDto? Map(Order? order) => order is null ? null :
            new(order.Id, order.OrderNumber, order.OrderDate, order.DeliveryDate,
                order.CustomerName, order.ShippingAddress, order.Phone, order.Email,
                order.Status, order.TotalAmount, order.TaxAmount, order.DiscountAmount,
                order.Notes,
                order.Items?.Select(i => new OrderItemDto(
                    i.Id, i.ProductId, i.Product?.Name, i.Product?.SKU,
                    i.Quantity, i.UnitPrice, i.TotalPrice, i.Discount, i.Unit)).ToList() ?? new());
    }
}
