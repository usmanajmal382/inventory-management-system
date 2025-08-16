using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.DTOs
{

    public record ProductDto(
        int Id,
        string Name,
        string? Description,
        string SKU,
        decimal Price,
        int Quantity,
        int MinStockLevel,
        int CategoryId,
        string? CategoryName,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        bool IsLowStock);

}
