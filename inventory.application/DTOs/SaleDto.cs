using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.DTOs
{
 
}
public record SaleDto(
    int Id,
    string SaleNumber,
    DateTime SaleDate,
    string? CustomerName,
    decimal TotalAmount,
    decimal TaxAmount,
    decimal DiscountAmount,
    List<SaleItemDto> Items);

public record SaleItemDto(
    int Id,
    int ProductId,
    string? ProductName,
    string? ProductSKU,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice,
    string? Unit);

public record DailySalesReportDto(
    DateTime Date,
    int TotalSales,
    decimal TotalRevenue,
    decimal TotalTax,
    decimal TotalDiscount);
