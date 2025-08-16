using inventory.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.DTOs
{
    public record StockTransactionDto(
    int Id,
    int ProductId,
    string? ProductName,
    int Quantity,
    TransactionType Type,
    string? Notes,
    DateTime CreatedAt);

    public record CreateStockTransactionDto(
        int ProductId,
        int Quantity,
        TransactionType Type,
        string? Notes);
}
