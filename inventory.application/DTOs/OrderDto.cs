using inventory.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.DTOs
{
    public record OrderDto(
        int Id,
        string OrderNumber,
        DateTime OrderDate,
        DateTime? DeliveryDate,
        string? CustomerName,
        string? ShippingAddress,
        string? Phone,
        string? Email,
        OrderStatus Status,
        decimal TotalAmount,
        decimal TaxAmount,
        decimal DiscountAmount,
        string? Notes,
        List<OrderItemDto> Items);

    public record OrderItemDto(
        int Id,
        int ProductId,
        string? ProductName,
        string? ProductSKU,
        int Quantity,
        decimal UnitPrice,
        decimal TotalPrice,
        decimal Discount,
        string? Unit);

    public record CreateOrderDto(
        DateTime? DeliveryDate,
        string? CustomerName,
        string? ShippingAddress,
        string? Phone,
        string? Email,
        decimal TaxAmount,
        decimal DiscountAmount,
        string? Notes,
        List<CreateOrderItemDto> Items);

    public record CreateOrderItemDto(
        int ProductId,
        int Quantity,
        decimal UnitPrice,
        decimal Discount,
        string? Unit);
}
