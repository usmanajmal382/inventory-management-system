using inventory.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.DTOs
{
    public record StockAlertDto(
    int Id,
    int ProductId,
    string? ProductName,
    string? ProductSKU,
    StockAlertType Type,
    string Message,
    bool IsRead,
    DateTime CreatedAt);
}
