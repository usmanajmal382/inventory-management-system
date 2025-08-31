using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.core.Entities
{

    public class Product
    {
        public int Id { get; set; }

        [Required, StringLength(100)] public string Name { get; set; } = default!;
        [StringLength(500)] public string? Description { get; set; }
        [Required, StringLength(50)] public string SKU { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int MinStockLevel { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
        public ICollection<ProductSupplier> ProductSuppliers { get; set; } = new List<ProductSupplier>();
        public ICollection<StockAlert> StockAlerts { get; set; } = new List<StockAlert>();
        public int? UnitId { get; set; }
        public Unit? Unit { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    }
}
