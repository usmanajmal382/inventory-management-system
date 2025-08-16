using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.core.Entities
{
    public class StockTransaction
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public TransactionType Type { get; set; }
        [StringLength(500)] public string? Notes { get; set; }
        [StringLength(100)] public string? ReferenceNumber { get; set; }
        public int? PurchaseOrderId { get; set; }
        public PurchaseOrder? PurchaseOrder { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum TransactionType
    {
        StockIn = 1,
        StockOut = 2,
        Adjustment = 3,
        PurchaseReceived = 4,
        Sale = 5,
        Return = 6,
        Damaged = 7
    }
}
