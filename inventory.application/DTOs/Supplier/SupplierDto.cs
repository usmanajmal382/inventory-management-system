using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.application.DTOs
{
    public record SupplierDto(
        int Id,
        string Name,
        string ContactPerson,
        string Phone,
        string Email,
        string Address,
        bool IsActive,
        int TotalOrders,
        decimal TotalOrderValue);
}
