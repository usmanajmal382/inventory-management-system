using inventory.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.DTOs
{
    public record PurchaseOrderDto(
     int Id,
     string OrderNumber,
     int SupplierId,
     string? SupplierName,
     DateTime OrderDate,
     DateTime? DeliveryDate,
     PurchaseOrderStatus Status,
     decimal TotalAmount,
     string? Notes,
     List<PurchaseOrderItemDto> Items);

    public record PurchaseOrderItemDto(
        int Id,
        int ProductId,
        string? ProductName,
        string? ProductSKU,
        int Quantity,
        decimal UnitPrice,
        decimal TotalPrice);

    public record CreatePurchaseOrderDto(
        int SupplierId,
        DateTime OrderDate,
        DateTime? DeliveryDate,
        string? Notes,
        List<CreatePurchaseOrderItemDto> Items);

    public record CreatePurchaseOrderItemDto(
        int ProductId,
        int Quantity,
        decimal UnitPrice);
}
