using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.core.Entities
{
    public class StockAlert
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public StockAlertType Type { get; set; }
        [StringLength(500)] public string Message { get; set; } = default!;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum StockAlertType
    {
        LowStock = 1,
        OutOfStock = 2,
        Overstock = 3
    }
}
