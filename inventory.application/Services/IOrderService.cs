using inventory.application.DTOs;
using inventory.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.Services
{
    public interface IOrderService
    {
        Task<OrderDto?> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<IEnumerable<OrderDto>> GetOrdersByStatusAsync(OrderStatus status);
        Task<IEnumerable<OrderDto>> GetOrdersByCustomerAsync(string customerName);
        Task<OrderDto> CreateOrderAsync(CreateOrderDto dto);
        Task<OrderDto> UpdateOrderStatusAsync(int id, OrderStatus status);
        Task<OrderDto> ConfirmOrderAsync(int id);
        Task<OrderDto> ShipOrderAsync(int id);
        Task<OrderDto> DeliverOrderAsync(int id);
        Task<OrderDto> CancelOrderAsync(int id);
        Task DeleteOrderAsync(int id);
    }
}
