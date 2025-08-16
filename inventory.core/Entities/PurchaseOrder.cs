using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.core.Entities
{
    public class PurchaseOrder
    {
        public int Id { get; set; }

        [Required, StringLength(50)] public string OrderNumber { get; set; } = default!;
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? DeliveryDate { get; set; }
        public PurchaseOrderStatus Status { get; set; } = PurchaseOrderStatus.Draft;
        public decimal TotalAmount { get; set; }
        [StringLength(500)] public string? Notes { get; set; }

        public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
    }

    public enum PurchaseOrderStatus
    {
        Draft = 1,
        Pending = 2,
        Approved = 3,
        Received = 4,
        Cancelled = 5
    }
}
